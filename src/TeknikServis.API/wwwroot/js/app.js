// Sayfa yüklendiğinde
document.addEventListener('DOMContentLoaded', function() {
    initApp();
});

// Uygulama başlatma
function initApp() {
    initSidebar();
    displayUserInfo();
}

// Sidebar işlemleri
function initSidebar() {
    const menuToggle = document.querySelector('.menu-toggle');
    const sidebar = document.querySelector('.sidebar');
    const overlay = document.querySelector('.sidebar-overlay');
    
    if (menuToggle) {
        menuToggle.addEventListener('click', toggleSidebar);
    }
    
    if (overlay) {
        overlay.addEventListener('click', toggleSidebar);
    }
    
    // Sayfa linkine tıklayınca mobilde sidebar'ı kapat
    const menuLinks = document.querySelectorAll('.sidebar-menu a');
    menuLinks.forEach(link => {
        link.addEventListener('click', () => {
            if (window.innerWidth <= 768) {
                closeSidebar();
            }
        });
    });
    
    // Aktif menü öğesini işaretle
    highlightActiveMenu();
}

// Sidebar aç/kapat
function toggleSidebar() {
    const sidebar = document.getElementById('sidebar') || document.querySelector('.sidebar');
    const overlay = document.querySelector('.sidebar-overlay');
    
    if (sidebar) {
        sidebar.classList.toggle('active');
    }
    if (overlay) {
        overlay.classList.toggle('active');
    }
    
    // Body scroll'u kontrol et
    if (sidebar && sidebar.classList.contains('active')) {
        document.body.style.overflow = 'hidden';
    } else {
        document.body.style.overflow = '';
    }
}

// Sidebar'ı kapat
function closeSidebar() {
    const sidebar = document.getElementById('sidebar') || document.querySelector('.sidebar');
    const overlay = document.querySelector('.sidebar-overlay');
    
    if (sidebar) {
        sidebar.classList.remove('active');
    }
    if (overlay) {
        overlay.classList.remove('active');
    }
    document.body.style.overflow = '';
}

// Aktif menü öğesini işaretle
function highlightActiveMenu() {
    const currentPath = window.location.pathname;
    const currentPage = currentPath.split('/').pop();
    const menuLinks = document.querySelectorAll('.sidebar-menu a');
    
    menuLinks.forEach(link => {
        link.classList.remove('active');
        const href = link.getAttribute('href');
        if (href && href !== '#' && currentPage === href) {
            link.classList.add('active');
        }
    });
}

// Kullanıcı bilgilerini sidebar'da göster
function displayUserInfo() {
    const user = Auth.getUser();
    if (!user) return;
    
    const userNameEl = document.getElementById('user-name');
    const userRoleEl = document.getElementById('user-role');
    const dukkanNameEl = document.getElementById('dukkan-name');
    
    if (userNameEl) userNameEl.textContent = user.adSoyad;
    if (userRoleEl) userRoleEl.textContent = user.rol === 'SuperAdmin' ? 'Sistem Yöneticisi' : 'Dükkan Kullanıcısı';
    if (dukkanNameEl) dukkanNameEl.textContent = user.dukkanAdi || 'Sistem Yönetimi';
}

// Sidebar HTML oluştur (Dükkan kullanıcısı için)
function getDukkanSidebarHTML() {
    const user = Auth.getUser();
    return `
        <div class="sidebar-header">
            <div class="sidebar-logo">
                <div class="sidebar-logo-icon">🔧</div>
                <h2>Teknik Servis</h2>
            </div>
            <div class="sidebar-user">
                <div class="sidebar-user-name" id="user-name">${user?.adSoyad || '-'}</div>
                <div class="sidebar-user-role" id="dukkan-name">${user?.dukkanAdi || '-'}</div>
            </div>
        </div>
        <ul class="sidebar-menu">
            <li><a href="dashboard.html"><span class="sidebar-menu-icon">📊</span> Dashboard</a></li>
            <li><a href="is-emirleri.html"><span class="sidebar-menu-icon">📋</span> İş Emirleri</a></li>
            <li><a href="musteriler.html"><span class="sidebar-menu-icon">👥</span> Müşteriler</a></li>
            <li><a href="cihazlar.html"><span class="sidebar-menu-icon">📱</span> Cihazlar</a></li>
            <li><a href="#" onclick="logout(); return false;"><span class="sidebar-menu-icon">🚪</span> Çıkış</a></li>
        </ul>
    `;
}

// Sidebar HTML oluştur (SuperAdmin için)
function getAdminSidebarHTML() {
    const user = Auth.getUser();
    return `
        <div class="sidebar-header">
            <div class="sidebar-logo">
                <div class="sidebar-logo-icon">⚙️</div>
                <h2>Yönetim Paneli</h2>
            </div>
            <div class="sidebar-user">
                <div class="sidebar-user-name" id="user-name">${user?.adSoyad || '-'}</div>
                <div class="sidebar-user-role">Sistem Yöneticisi</div>
            </div>
        </div>
        <ul class="sidebar-menu">
            <li><a href="talepler.html"><span class="sidebar-menu-icon">📝</span> Kayıt Talepleri</a></li>
            <li><a href="dukkanlar.html"><span class="sidebar-menu-icon">🏪</span> Dükkanlar</a></li>
            <li><a href="#" onclick="logout(); return false;"><span class="sidebar-menu-icon">🚪</span> Çıkış</a></li>
        </ul>
    `;
}