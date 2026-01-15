// API Ayarları
const CONFIG = {
    API_URL: 'http://localhost:5226/api',
    
    // Token süresi (saat)
    TOKEN_EXPIRE_HOURS: 24,
    
    // Sayfalama
    DEFAULT_PAGE_SIZE: 20,
    
    // Öncelik seviyeleri
    PRIORITIES: {
        0: { text: 'Düşük', class: 'badge-success', color: '#27ae60' },
        1: { text: 'Normal', class: 'badge-primary', color: '#3498db' },
        2: { text: 'Acil', class: 'badge-danger', color: '#e74c3c' }
    },
    
    // Müşteri tipleri
    CUSTOMER_TYPES: {
        0: { text: 'Bireysel', class: 'badge-primary' },
        1: { text: 'Kurumsal', class: 'badge-warning' }
    },
    
    // Kayıt talep durumları
    REQUEST_STATUS: {
        0: { text: 'Beklemede', class: 'badge-warning' },
        1: { text: 'Onaylandı', class: 'badge-success' },
        2: { text: 'Reddedildi', class: 'badge-danger' }
    },
    
    // Teklif durumları
    OFFER_STATUS: {
        0: { text: 'Bekliyor', class: 'badge-warning' },
        1: { text: 'Onaylandı', class: 'badge-success' },
        2: { text: 'Reddedildi', class: 'badge-danger' }
    },
    
    // Kalem tipleri
    ITEM_TYPES: {
        0: { text: 'İşçilik' },
        1: { text: 'Parça' }
    }
};

// Sayfa yolları
const ROUTES = {
    LOGIN: '/index.html',
    DASHBOARD: '/pages/dashboard.html',
    CUSTOMERS: '/pages/musteriler.html',
    DEVICES: '/pages/cihazlar.html',
    WORK_ORDERS: '/pages/is-emirleri.html',
    WORK_ORDER_DETAIL: '/pages/is-emri-detay.html',
    ADMIN_REQUESTS: '/pages/admin/talepler.html',
    ADMIN_SHOPS: '/pages/admin/dukkanlar.html'
};