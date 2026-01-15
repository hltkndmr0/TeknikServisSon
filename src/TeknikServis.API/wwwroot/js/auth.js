// Token ve kullanıcı işlemleri
const Auth = {
    // Token al
    getToken() {
        return localStorage.getItem('token');
    },
    
    // Token kaydet
    setToken(token) {
        localStorage.setItem('token', token);
    },
    
    // Token sil
    removeToken() {
        localStorage.removeItem('token');
    },
    
    // Kullanıcı bilgisi al
    getUser() {
        const user = localStorage.getItem('user');
        return user ? JSON.parse(user) : null;
    },
    
    // Kullanıcı bilgisi kaydet
    setUser(user) {
        localStorage.setItem('user', JSON.stringify(user));
    },
    
    // Kullanıcı bilgisi sil
    removeUser() {
        localStorage.removeItem('user');
    },
    
    // Giriş yapılmış mı?
    isLoggedIn() {
        const token = this.getToken();
        if (!token) return false;
        
        // Token süresini kontrol et
        const tokenExpiry = localStorage.getItem('tokenExpiry');
        if (tokenExpiry && new Date(tokenExpiry) < new Date()) {
            this.logout();
            return false;
        }
        
        return true;
    },
    
    // Token süresini kaydet
    setTokenExpiry(expiry) {
        localStorage.setItem('tokenExpiry', expiry);
    },
    
    // SuperAdmin mi?
    isSuperAdmin() {
        const user = this.getUser();
        return user && user.rol === 'SuperAdmin';
    },
    
    // Dükkan kullanıcısı mı?
    isDukkanUser() {
        const user = this.getUser();
        return user && user.rol === 'DukkanKullanici';
    },
    
    // Giriş yap
    login(data) {
        this.setToken(data.token);
        this.setUser(data.kullanici);
        this.setTokenExpiry(data.tokenExpiry);
    },
    
    // Çıkış yap
    logout() {
        this.removeToken();
        this.removeUser();
        localStorage.removeItem('tokenExpiry');
        window.location.href = ROUTES.LOGIN;
    },
    
    // Sayfa koruma - giriş yapmamışsa login'e yönlendir
    requireAuth() {
        if (!this.isLoggedIn()) {
            window.location.href = ROUTES.LOGIN;
            return false;
        }
        return true;
    },
    
    // SuperAdmin kontrolü
    requireSuperAdmin() {
        if (!this.requireAuth()) return false;
        
        if (!this.isSuperAdmin()) {
            window.location.href = ROUTES.DASHBOARD;
            return false;
        }
        return true;
    },
    
    // Dükkan kullanıcısı kontrolü
    requireDukkanUser() {
        if (!this.requireAuth()) return false;
        
        if (!this.isDukkanUser()) {
            window.location.href = ROUTES.ADMIN_REQUESTS;
            return false;
        }
        return true;
    }
};