async function apiRequest(endpoint, options = {}) {
    const url = `${CONFIG.API_URL}${endpoint}`;
    
    const defaultHeaders = {
        'Content-Type': 'application/json'
    };
    
    const token = Auth.getToken();
    if (token) {
        defaultHeaders['Authorization'] = `Bearer ${token}`;
    }
    
    const config = {
        ...options,
        headers: {
            ...defaultHeaders,
            ...options.headers
        }
    };
    
    if (options.body && typeof options.body === 'object') {
        config.body = JSON.stringify(options.body);
    }
    
    try {
        const response = await fetch(url, config);
        
        // Hata durumunda detayı logla
        if (!response.ok) {
            const errorData = await response.json().catch(() => null);
            console.error('API Hatası:', response.status, errorData);
            
            if (response.status === 401) {
                Auth.logout();
                return null;
            }
            
            // Validation hataları için
            if (errorData?.errors) {
                const firstError = Object.values(errorData.errors)[0];
                return { success: false, errors: [Array.isArray(firstError) ? firstError[0] : firstError] };
            }
            
            return errorData || { success: false, message: 'Bir hata oluştu' };
        }
        
        const data = await response.json();
        return data;
        
    } catch (error) {
        console.error('API Hatası:', error);
        showToast('Sunucu ile bağlantı kurulamadı', 'error');
        throw error;
    }
}

// ==================== AUTH API ====================
const AuthAPI = {
    async login(firmaKodu, email, sifre) {
        return await apiRequest('/auth/login', {
            method: 'POST',
            body: { firmaKodu, email, sifre }
        });
    },
    
    async adminLogin(email, sifre) {
        return await apiRequest('/auth/admin/login', {
            method: 'POST',
            body: { email, sifre }
        });
    },
    
    async kayitTalep(data) {
        return await apiRequest('/auth/kayit-talep', {
            method: 'POST',
            body: data
        });
    },
    
    async getBekleyenTalepler() {
        return await apiRequest('/auth/kayit-talep/bekleyenler');
    },
    
    async getTumTalepler(durum = null, page = 1, pageSize = CONFIG.DEFAULT_PAGE_SIZE) {
        let url = `/auth/kayit-talep?page=${page}&pageSize=${pageSize}`;
        if (durum !== null) url += `&durum=${durum}`;
        return await apiRequest(url);
    },
    
    async getTalepById(id) {
        return await apiRequest(`/auth/kayit-talep/${id}`);
    },
    
    async talepOnayla(talepId, notlar = '') {
        return await apiRequest('/auth/kayit-talep/onayla', {
            method: 'POST',
            body: { talepId, notlar }
        });
    },
    
    async talepReddet(talepId, redNedeni) {
        return await apiRequest('/auth/kayit-talep/reddet', {
            method: 'POST',
            body: { talepId, redNedeni }
        });
    }
};

// ==================== MÜŞTERİLER API ====================
const MusterilerAPI = {
    async getAll(arama = '', page = 1, pageSize = CONFIG.DEFAULT_PAGE_SIZE) {
        return await apiRequest(`/musteriler?arama=${encodeURIComponent(arama)}&page=${page}&pageSize=${pageSize}`);
    },
    
    async search(params) {
        const query = new URLSearchParams(params).toString();
        return await apiRequest(`/musteriler/ara?${query}`);
    },
    
    async getById(id) {
        return await apiRequest(`/musteriler/${id}`);
    },
    
    async create(data) {
        return await apiRequest('/musteriler', {
            method: 'POST',
            body: data
        });
    },
    
    async update(data) {
        return await apiRequest('/musteriler', {
            method: 'PUT',
            body: data
        });
    },
    
    async delete(id) {
        return await apiRequest(`/musteriler/${id}`, {
            method: 'DELETE'
        });
    }
};

// ==================== CİHAZLAR API ====================
const CihazlarAPI = {
    async getKategoriler() {
        return await apiRequest('/cihazlar/kategoriler');
    },
    
    async createKategori(ad) {
        return await apiRequest('/cihazlar/kategoriler', {
            method: 'POST',
            body: { ad }
        });
    },
    
    async getTanimlar(kategoriId = null, arama = '') {
        let url = '/cihazlar/tanimlar?';
        if (kategoriId) url += `kategoriId=${kategoriId}&`;
        if (arama) url += `arama=${encodeURIComponent(arama)}`;
        return await apiRequest(url);
    },
    
    async createTanim(data) {
        return await apiRequest('/cihazlar/tanimlar', {
            method: 'POST',
            body: data
        });
    },
    
    // Tüm cihazları getir (yeni eklendi)
    async getAll(arama = '', page = 1, pageSize = CONFIG.DEFAULT_PAGE_SIZE) {
        let url = `/cihazlar?page=${page}&pageSize=${pageSize}`;
        if (arama) url += `&arama=${encodeURIComponent(arama)}`;
        return await apiRequest(url);
    },
    
    async getById(id) {
        return await apiRequest(`/cihazlar/${id}`);
    },
    
    async getByMusteri(musteriId) {
        return await apiRequest(`/cihazlar/musteri/${musteriId}`);
    },
    
    async getBySeriNo(seriNo) {
        return await apiRequest(`/cihazlar/seri/${encodeURIComponent(seriNo)}`);
    },
    
    async create(data) {
        return await apiRequest('/cihazlar', {
            method: 'POST',
            body: data
        });
    },
    
    async update(data) {
        return await apiRequest('/cihazlar', {
            method: 'PUT',
            body: data
        });
    }
};

// ==================== İŞ EMİRLERİ API ====================
const IsEmirleriAPI = {
    async getDurumlar() {
        return await apiRequest('/isemirleri/durumlar');
    },
    
    async getAll(params = {}) {
        const { durumId, oncelik, arama, page = 1, pageSize = CONFIG.DEFAULT_PAGE_SIZE } = params;
        let url = `/isemirleri?page=${page}&pageSize=${pageSize}`;
        if (durumId) url += `&durumId=${durumId}`;
        if (oncelik !== null && oncelik !== undefined) url += `&oncelik=${oncelik}`;
        if (arama) url += `&arama=${encodeURIComponent(arama)}`;
        return await apiRequest(url);
    },
    
    async getById(id) {
        return await apiRequest(`/isemirleri/${id}`);
    },
    
    async getByDurum(durumId) {
        return await apiRequest(`/isemirleri/durum/${durumId}`);
    },
    
    async getByMusteri(musteriId) {
        return await apiRequest(`/isemirleri/musteri/${musteriId}`);
    },
    
    async create(data) {
        return await apiRequest('/isemirleri', {
            method: 'POST',
            body: data
        });
    },
    
    async update(data) {
        return await apiRequest('/isemirleri', {
            method: 'PUT',
            body: data
        });
    },
    
    async updateDurum(isEmriId, durumId, aciklama = '') {
        return await apiRequest('/isemirleri/durum', {
            method: 'PATCH',
            body: { isEmriId, durumId, aciklama }
        });
    },
    
    async addNot(isEmriId, notMetni) {
        return await apiRequest('/isemirleri/not', {
            method: 'POST',
            body: { isEmriId, notMetni }
        });
    }
};

// ==================== TEKLİFLER API ====================
const TekliflerAPI = {
    async getById(id) {
        return await apiRequest(`/teklifler/${id}`);
    },
    
    async getByIsEmri(isEmriId) {
        return await apiRequest(`/teklifler/isemri/${isEmriId}`);
    },
    
    async create(data) {
        return await apiRequest('/teklifler', {
            method: 'POST',
            body: data
        });
    },
    
    async onayla(teklifId) {
        return await apiRequest('/teklifler/onayla', {
            method: 'POST',
            body: { teklifId }
        });
    },
    
    async reddet(teklifId) {
        return await apiRequest('/teklifler/reddet', {
            method: 'POST',
            body: { teklifId }
        });
    },
    
    getPdfUrl(id) {
        return `${CONFIG.API_URL}/teklifler/${id}/pdf`;
    }
};

// ==================== FATURALAR API ====================
const FaturalarAPI = {
    async getById(id) {
        return await apiRequest(`/faturalar/${id}`);
    },
    
    async getByIsEmri(isEmriId) {
        return await apiRequest(`/faturalar/isemri/${isEmriId}`);
    },
    
    async create(data) {
        return await apiRequest('/faturalar', {
            method: 'POST',
            body: data
        });
    },
    
    getPdfUrl(id) {
        return `${CONFIG.API_URL}/faturalar/${id}/pdf`;
    }
};

// ==================== DÜKKANLAR API (SuperAdmin) ====================
const DukkanlarAPI = {
    async getAll(arama = '', aktif = null, page = 1, pageSize = CONFIG.DEFAULT_PAGE_SIZE) {
        let url = `/dukkanlar?page=${page}&pageSize=${pageSize}`;
        if (arama) url += `&arama=${encodeURIComponent(arama)}`;
        if (aktif !== null) url += `&aktif=${aktif}`;
        return await apiRequest(url);
    },
    
    async getById(id) {
        return await apiRequest(`/dukkanlar/${id}`);
    },
    
    async update(data) {
        return await apiRequest('/dukkanlar', {
            method: 'PUT',
            body: data
        });
    },
    
    async delete(id) {
        return await apiRequest(`/dukkanlar/${id}`, {
            method: 'DELETE'
        });
    }
};