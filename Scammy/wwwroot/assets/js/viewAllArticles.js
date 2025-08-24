document.addEventListener('DOMContentLoaded', function () {
    // Elements
    const searchInput = document.getElementById('articleSearch');
    const categoryFilter = document.getElementById('categoryFilter');
    const sortFilter = document.getElementById('sortFilter');
    const viewBtns = document.querySelectorAll('.view-btn');
    const articlesContainer = document.getElementById('articlesContainer');
    const articlesCount = document.getElementById('articlesCount');
    const backToTopBtn = document.getElementById('backToTop');
    const loadMoreBtn = document.getElementById('loadMoreBtn');
    
    

    let allArticles = Array.from(document.querySelectorAll('.article-card'));
    let filteredArticles = [...allArticles];
    let articlesPerPage = 12;
    let currentPage = 1;

    // Initialize
    updateArticlesCount();
    showArticles();

    // Search functionality
    if (searchInput) {
        searchInput.addEventListener('input', debounce(handleSearch, 300));
    }

    // Filter functionality
    if (categoryFilter) {
        categoryFilter.addEventListener('change', handleFilter);
    }

    // Sort functionality
    if (sortFilter) {
        sortFilter.addEventListener('change', handleSort);
    }

    // View toggle
    viewBtns.forEach(btn => {
        btn.addEventListener('click', handleViewToggle);
    });

    // Back to top button
    window.addEventListener('scroll', handleScroll);
    if (backToTopBtn) {
        backToTopBtn.addEventListener('click', scrollToTop);
    }

    // Load more functionality
    if (loadMoreBtn) {
        loadMoreBtn.addEventListener('click', loadMoreArticles);
    }


    // Functions
    function handleSearch() {
        const query = searchInput.value.toLowerCase().trim();

        filteredArticles = allArticles.filter(article => {
            const title = article.getAttribute('data-title') || '';
            const content = article.getAttribute('data-content') || '';
            const category = article.getAttribute('data-category') || '';

            return title.includes(query) ||
                content.includes(query) ||
                category.toLowerCase().includes(query);
        });

        currentPage = 1;
        updateArticlesCount();
        showArticles();
    }

    function handleFilter() {
        const selectedCategory = categoryFilter.value;

        filteredArticles = allArticles.filter(article => {
            if (!selectedCategory) return true;
            return article.getAttribute('data-category') === selectedCategory;
        });

        // Apply search if there's a search query
        if (searchInput && searchInput.value.trim()) {
            handleSearch();
            return;
        }

        currentPage = 1;
        updateArticlesCount();
        showArticles();
    }

    function handleSort() {
        const sortBy = sortFilter.value;

        

        filteredArticles.sort((a, b) => {
            switch (sortBy) {
                case 'newest':
                    return new Date(b.getAttribute('data-date')) - new Date(a.getAttribute('data-date'));
                case 'oldest':
                    return new Date(a.getAttribute('data-date')) - new Date(b.getAttribute('data-date'));
                case 'title':
                    return a.getAttribute('data-title').localeCompare(b.getAttribute('data-title'));
                case 'category':
                    return a.getAttribute('data-category').localeCompare(b.getAttribute('data-category'));
                default:
                    return 0;
            }
        });
       
        

        
        const container = document.getElementById("articlesContainer");
        container.innerHTML = ""; // clear old order
        filteredArticles.forEach(article => container.appendChild(article));

        
    }

    
    function handleViewToggle(e) {
        const viewType = e.currentTarget.getAttribute('data-view');

        // Update active state
        viewBtns.forEach(btn => btn.classList.remove('active'));
        e.currentTarget.classList.add('active');

        // Update container class
        if (viewType === 'list') {
            articlesContainer.classList.add('list-view');
        } else {
            articlesContainer.classList.remove('list-view');
        }
    }

    function showArticles() {
        // Hide all articles first
        allArticles.forEach(article => {
            article.style.display = 'none';
            article.classList.remove('fade-in');
        });

        // Show filtered articles with pagination
        const startIndex = 0;
        const endIndex = currentPage * articlesPerPage;
        const articlesToShow = filteredArticles.slice(startIndex, endIndex);

        articlesToShow.forEach((article, index) => {
            article.style.display = 'block';
            setTimeout(() => {
                article.classList.add('fade-in');
            }, index * 50);
        });

        // Show/hide load more button
        const loadMoreSection = document.getElementById('loadMoreSection');
        if (loadMoreSection) {
            if (filteredArticles.length > endIndex) {
                loadMoreSection.style.display = 'block';
            } else {
                loadMoreSection.style.display = 'none';
            }
        }

        // Scroll to top of articles section on filter/search
        if (currentPage === 1) {
            const articlesSection = document.querySelector('.articles-content');
            if (articlesSection) {
                articlesSection.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }
        }
    }

    function loadMoreArticles() {
        currentPage++;
        showArticles();

        // Scroll to the new articles
        setTimeout(() => {
            const newArticles = filteredArticles.slice((currentPage - 1) * articlesPerPage, currentPage * articlesPerPage);
            if (newArticles.length > 0) {
                newArticles[0].scrollIntoView({ behavior: 'smooth', block: 'center' });
            }
        }, 100);
    }

    function updateArticlesCount() {
        if (articlesCount) {
            const count = filteredArticles.length;
            const total = allArticles.length;

            if (count === total) {
                articlesCount.textContent = `Showing ${count} articles`;
            } else {
                articlesCount.textContent = `Showing ${count} of ${total} articles`;
            }
        }
    }

    function handleScroll() {
        if (backToTopBtn) {
            if (window.pageYOffset > 300) {
                backToTopBtn.classList.add('show');
            } else {
                backToTopBtn.classList.remove('show');
            }
        }

        // Update sticky filter background
        const filterSection = document.querySelector('.articles-filter');
        if (filterSection) {
            if (window.pageYOffset > 100) {
                filterSection.style.background = 'rgba(255, 255, 255, 0.95)';
                filterSection.style.backdropFilter = 'blur(10px)';
            } else {
                filterSection.style.background = 'white';
                filterSection.style.backdropFilter = 'none';
            }
        }
    }

    function scrollToTop() {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    }

  

    
   

    function showToast(message, type = 'info') {
        // Create toast element
        const toast = document.createElement('div');
        toast.className = `toast toast-${type}`;
        toast.innerHTML = `
            <div class="toast-content">
                <i class="fas fa-${getToastIcon(type)}"></i>
                <span>${message}</span>
            </div>
        `;

        // Add toast styles if not already added
        if (!document.querySelector('.toast-styles')) {
            const styles = document.createElement('style');
            styles.className = 'toast-styles';
            styles.textContent = `
                .toast {
                    position: fixed;
                    top: 20px;
                    right: 20px;
                    background: white;
                    border-radius: 8px;
                    box-shadow: 0 4px 12px rgba(0,0,0,0.15);
                    z-index: 10000;
                    transform: translateX(100%);
                    transition: transform 0.3s ease;
                    border-left: 4px solid;
                }
                .toast.show { transform: translateX(0); }
                .toast-success { border-left-color: #10b981; }
                .toast-error { border-left-color: #ef4444; }
                .toast-info { border-left-color: #3b82f6; }
                .toast-content {
                    padding: 16px 20px;
                    display: flex;
                    align-items: center;
                    gap: 12px;
                    color: #374151;
                }
                .toast-success .toast-content i { color: #10b981; }
                .toast-error .toast-content i { color: #ef4444; }
                .toast-info .toast-content i { color: #3b82f6; }
            `;
            document.head.appendChild(styles);
        }

        document.body.appendChild(toast);

        // Show toast
        setTimeout(() => toast.classList.add('show'), 100);

        // Hide toast after 3 seconds
        setTimeout(() => {
            toast.classList.remove('show');
            setTimeout(() => toast.remove(), 300);
        }, 3000);
    }

    function getToastIcon(type) {
        const icons = {
            success: 'check-circle',
            error: 'exclamation-circle',
            info: 'info-circle'
        };
        return icons[type] || 'info-circle';
    }

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

    // Smooth scrolling for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });
});

// Share article function (global scope for onclick handlers)
function shareArticle(title, url) {
    if (navigator.share) {
        navigator.share({
            title: title,
            url: url
        }).catch(console.error);
    } else {
        // Fallback: copy to clipboard
        const fullUrl = window.location.origin + url;
        navigator.clipboard.writeText(fullUrl).then(() => {
            showToast('Article link copied to clipboard!', 'success');
        }).catch(() => {
            // Fallback for older browsers
            const textArea = document.createElement('textarea');
            textArea.value = fullUrl;
            document.body.appendChild(textArea);
            textArea.select();
            document.execCommand('copy');
            document.body.removeChild(textArea);
            showToast('Article link copied to clipboard!', 'success');
        });
    }
}