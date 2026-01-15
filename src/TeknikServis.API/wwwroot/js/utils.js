// Yardımcı fonksiyonlar

// Toast bildirimi göster
function showToast(message, type = 'success') {
    let container = document.querySelector('.toast-container');
    
    if (!container) {
        container = document.createElement('div');
        container.className = 'toast-container';
        document.body.appendChild(container);
    }
    
    const toast = document.createElement('div');
    toast.className = `toast toast-${type}`;
    toast.textContent = message;
    
    container.appendChild(toast);
    
    // 3 saniye sonra kaldır
    setTimeout(() => {
        toast.style.animation = 'toastSlideOut 0.3s ease forwards';
        setTimeout(() => toast.remove(), 300);
    }, 3000);
}

// Loading göster
function showLoading(container) {
    if (typeof container === 'string') {
        container = document.querySelector(container);
    }
    
    if (container) {
        container.innerHTML = `
            <div class="loading">
                <div class="spinner"></div>
                <p>Yükleniyor...</p>
            </div>
        `;
    }
}

// Boş durum göster
function showEmptyState(container, message = 'Kayıt bulunamadı', icon = '📭') {
    if (typeof container === 'string') {
        container = document.querySelector(container);
    }
    
    if (container) {
        container.innerHTML = `
            <div class="empty-state">
                <div class="empty-state-icon">${icon}</div>
                <h3>${message}</h3>
            </div>
        `;
    }
}

// Modal aç
function openModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.classList.add('active');
        document.body.style.overflow = 'hidden';
    }
}

// Modal kapat
function closeModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.classList.remove('active');
        document.body.style.overflow = '';
    }
}

// Tüm modallara kapatma işlevi ekle
document.addEventListener('click', (e) => {
    // Modal dışına tıklandığında kapat
    if (e.target.classList.contains('modal')) {
        e.target.classList.remove('active');
        document.body.style.overflow = '';
    }
    
    // Kapatma butonuna tıklandığında
    if (e.target.classList.contains('modal-close')) {
        const modal = e.target.closest('.modal');
        if (modal) {
            modal.classList.remove('active');
            document.body.style.overflow = '';
        }
    }
});

// Form verilerini objeye çevir
function getFormData(formId) {
    const form = document.getElementById(formId);
    if (!form) return {};
    
    const formData = new FormData(form);
    const data = {};
    
    formData.forEach((value, key) => {
        if (value === '') {
            data[key] = null;
        } else if (!isNaN(value) && value.trim() !== '') {
            data[key] = Number(value);
        } else {
            data[key] = value;
        }
    });
    
    return data;
}

// Formu temizle
function resetForm(formId) {
    const form = document.getElementById(formId);
    if (form) {
        form.reset();
        // Hidden inputları da temizle
        form.querySelectorAll('input[type="hidden"]').forEach(input => {
            if (input.name !== '_method') input.value = '';
        });
    }
}

// Formu doldur
function fillForm(formId, data) {
    const form = document.getElementById(formId);
    if (!form || !data) return;
    
    Object.keys(data).forEach(key => {
        const input = form.querySelector(`[name="${key}"]`);
        if (input) {
            if (input.type === 'checkbox') {
                input.checked = !!data[key];
            } else {
                input.value = data[key] ?? '';
            }
        }
    });
}

// Tarih formatla
function formatDate(dateString) {
    if (!dateString) return '-';
    
    const date = new Date(dateString);
    return date.toLocaleDateString('tr-TR', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
    });
}

// Tarih ve saat formatla
function formatDateTime(dateString) {
    if (!dateString) return '-';
    
    const date = new Date(dateString);
    return date.toLocaleDateString('tr-TR', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
}

// Para formatla
function formatMoney(amount) {
    if (amount === null || amount === undefined) return '-';
    
    return new Intl.NumberFormat('tr-TR', {
        style: 'currency',
        currency: 'TRY'
    }).format(amount);
}

// Telefon formatla
function formatPhone(phone) {
    if (!phone) return '-';
    
    const cleaned = phone.replace(/\D/g, '');
    
    if (cleaned.length === 10) {
        return `${cleaned.slice(0, 3)} ${cleaned.slice(3, 6)} ${cleaned.slice(6, 8)} ${cleaned.slice(8)}`;
    }
    
    return phone;
}

// Öncelik badge'i
function getPriorityBadge(priority) {
    const p = CONFIG.PRIORITIES[priority] || CONFIG.PRIORITIES[1];
    return `<span class="badge ${p.class}">${p.text}</span>`;
}

// Müşteri tipi badge'i
function getCustomerTypeBadge(type) {
    const t = CONFIG.CUSTOMER_TYPES[type] || CONFIG.CUSTOMER_TYPES[0];
    return `<span class="badge ${t.class}">${t.text}</span>`;
}

// Durum badge'i (dinamik renk)
function getStatusBadge(text, color) {
    return `<span class="badge" style="background-color: ${color}; color: white;">${text}</span>`;
}

// Kayıt talep durumu badge'i
function getRequestStatusBadge(status) {
    const s = CONFIG.REQUEST_STATUS[status] || CONFIG.REQUEST_STATUS[0];
    return `<span class="badge ${s.class}">${s.text}</span>`;
}

// Teklif durumu badge'i
function getOfferStatusBadge(status) {
    const s = CONFIG.OFFER_STATUS[status] || CONFIG.OFFER_STATUS[0];
    return `<span class="badge ${s.class}">${s.text}</span>`;
}

// Select'i doldur
function populateSelect(selectId, items, valueField, textField, placeholder = 'Seçiniz...') {
    const select = document.getElementById(selectId);
    if (!select) return;
    
    select.innerHTML = `<option value="">${placeholder}</option>`;
    
    items.forEach(item => {
        const option = document.createElement('option');
        option.value = item[valueField];
        option.textContent = typeof textField === 'function' ? textField(item) : item[textField];
        select.appendChild(option);
    });
}

// URL parametresi al
function getUrlParam(param) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(param);
}

// Debounce fonksiyonu (arama için)
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Onay dialogu
function confirmAction(message) {
    return confirm(message);
}

// Sayfalama HTML oluştur
function createPagination(currentPage, totalPages, onPageChange) {
    if (totalPages <= 1) return '';
    
    let html = '<div class="pagination">';
    
    // Önceki
    if (currentPage > 1) {
        html += `<button class="btn btn-sm" onclick="${onPageChange}(${currentPage - 1})">«</button>`;
    }
    
    // Sayfa numaraları
    for (let i = 1; i <= totalPages; i++) {
        if (i === currentPage) {
            html += `<button class="btn btn-sm btn-primary">${i}</button>`;
        } else if (i === 1 || i === totalPages || (i >= currentPage - 2 && i <= currentPage + 2)) {
            html += `<button class="btn btn-sm" onclick="${onPageChange}(${i})">${i}</button>`;
        } else if (i === currentPage - 3 || i === currentPage + 3) {
            html += `<span style="padding: 5px;">...</span>`;
        }
    }
    
    // Sonraki
    if (currentPage < totalPages) {
        html += `<button class="btn btn-sm" onclick="${onPageChange}(${currentPage + 1})">»</button>`;
    }
    
    html += '</div>';
    return html;
}

// Çıkış yap
function logout() {
    if (confirmAction('Çıkış yapmak istediğinize emin misiniz?')) {
        Auth.logout();
    }
}