document.addEventListener('DOMContentLoaded', function () {
    
    initializeSearch();
    initializeFiltering();
    initializeViewToggle();

    
    function initializeSearch() {
        const searchInput = document.getElementById('searchInput');
        const categoryFilter = document.getElementById('categoryFilter');

        if (searchInput) {
            searchInput.addEventListener('input', function () {
                filterArticles();
            });
        }

        if (categoryFilter) {
            categoryFilter.addEventListener('change', function () {
                filterArticles();
            });
        }
    }

    
    function filterArticles() {
        const searchTerm = document.getElementById('searchInput').value.toLowerCase();
        const selectedCategory = document.getElementById('categoryFilter').value;
        const articleCards = document.querySelectorAll('.article-card');

        let visibleCount = 0;

        articleCards.forEach(card => {
            const title = card.getAttribute('data-title') || '';
            const excerpt = card.getAttribute('data-excerpt') || '';
            const category = card.getAttribute('data-category') || '';

            const matchesSearch = title.includes(searchTerm) || excerpt.includes(searchTerm);
            const matchesCategory = selectedCategory === '' || category === selectedCategory;

            if (matchesSearch && matchesCategory) {
                card.classList.remove('hidden');
                card.classList.add('filtered-in');
                card.classList.remove('filtered-out');
                visibleCount++;
            } else {
                card.classList.add('filtered-out');
                card.classList.remove('filtered-in');
                setTimeout(() => {
                    card.classList.add('hidden');
                }, 300);
            }
        });

        
        updateEmptyState(visibleCount);
    }

    function updateEmptyState(visibleCount) {
        const articlesContainer = document.getElementById('articlesContainer');
        let emptyState = document.querySelector('.empty-state');

        if (visibleCount === 0 && !emptyState) {
            emptyState = document.createElement('div');
            emptyState.className = 'empty-state';
            emptyState.innerHTML = `
                <div class="empty-icon">
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <circle cx="11" cy="11" r="8"></circle>
                        <path d="m21 21-4.35-4.35"></path>
                    </svg>
                </div>
                <h3>No Articles Found</h3>
                <p>No published articles match your current search criteria.</p>
            `;
            articlesContainer.appendChild(emptyState);
        } else if (visibleCount > 0 && emptyState) {
            emptyState.remove();
        }
    }

    
    function initializeViewToggle() {
        const viewButtons = document.querySelectorAll('.view-btn');
        const articlesContainer = document.getElementById('articlesContainer');

        viewButtons.forEach(button => {
            button.addEventListener('click', function () {
               
                viewButtons.forEach(btn => btn.classList.remove('active'));
               
                this.classList.add('active');

                const view = this.getAttribute('data-view');

                if (view === 'list') {
                    articlesContainer.classList.add('list-view');
                } else {
                    articlesContainer.classList.remove('list-view');
                }
            });
        });
    }

   
    function initializeFiltering() {
        
        console.log('Filtering initialized');
    }
});


function previewArticle(title, content, category, author, date, adminComment) {
    const modal = document.getElementById('previewModal');
    const previewTitle = document.getElementById('previewTitle');
    const previewCategory = document.getElementById('previewCategory');
    const previewContent = document.getElementById('previewContent');
    const previewAdminSection = document.getElementById('previewAdminSection');
    const previewAdminComment = document.getElementById('previewAdminComment');
    const previewAuthor = document.getElementById('previewAuthor');  
    const previewDate = document.getElementById('previewDate');

    
    previewTitle.textContent = title;
    previewCategory.textContent = category;
    previewContent.textContent = content;
    previewAuthor.textContent = author;        
    previewDate.textContent = date;  

    previewContent.innerHTML = content;

    if (adminComment && adminComment.trim() !== '' && adminComment !== 'null') {
        previewAdminComment.textContent = adminComment;
        previewAdminSection.style.display = 'block';
    } else {
        previewAdminSection.style.display = 'none';
    }

   
    modal.style.display = 'block';
    document.body.style.overflow = 'hidden';

   
    modal.onclick = function (event) {
        if (event.target === modal) {
            closeModal();
        }
    }
}

function previewArticleData(button) {
    const title = button.getAttribute('data-title');
    const content = button.getAttribute('data-content');
    const category = button.getAttribute('data-category');
    const author = button.getAttribute('data-author');
    const date = button.getAttribute('data-date');
    const adminComment = button.getAttribute('data-admin-comment');

    const tempDiv = document.createElement('div');
    tempDiv.innerHTML = content;
    const decodedContent = tempDiv.innerHTML;

    previewArticle(title, decodedContent, category, author, date, adminComment);
}

function closeModal() {
    const modal = document.getElementById('previewModal');
    modal.style.display = 'none';
    document.body.style.overflow = 'auto';
}


document.addEventListener('keydown', function (event) {
    
    if (event.key === 'Escape') {
        closeModal();
    }

    
    if ((event.ctrlKey || event.metaKey) && event.key === 'f') {
        event.preventDefault();
        const searchInput = document.getElementById('searchInput');
        if (searchInput) {
            searchInput.focus();
            searchInput.select();
        }
    }
});

// Utility functions
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




// Smooth scroll to top
function scrollToTop() {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
}

// Add scroll to top button functionality
window.addEventListener('scroll', function () {
    const scrollButton = document.getElementById('scrollToTop');
    if (scrollButton) {
        if (window.pageYOffset > 300) {
            scrollButton.style.display = 'block';
        } else {
            scrollButton.style.display = 'none';
        }
    }
});

// Performance optimization for large lists
function virtualizeList() {
    
    console.log('List virtualization can be implemented here if needed');
}

// Analytics tracking (if needed)


// Article sharing functionality 
function shareArticle(title, url) {
    if (navigator.share) {
        navigator.share({
            title: title,
            url: url
        }).then(() => {
            console.log('Article shared successfully');
        }).catch((error) => {
            console.log('Error sharing article:', error);
        });
    } else {
        // Fallback for browsers that don't support Web Share API
        const tempInput = document.createElement('input');
        tempInput.value = url;
        document.body.appendChild(tempInput);
        tempInput.select();
        document.execCommand('copy');
        document.body.removeChild(tempInput);

        // Show notification
        showNotification('Link copied to clipboard!');
    }
}

function showNotification(message) {
    const notification = document.createElement('div');
    notification.className = 'notification';
    notification.textContent = message;
    notification.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        background: #10b981;
        color: white;
        padding: 12px 20px;
        border-radius: 8px;
        z-index: 1001;
        font-size: 14px;
        font-weight: 500;
        box-shadow: 0 4px 12px rgba(0,0,0,0.1);
        transform: translateX(100%);
        transition: transform 0.3s ease;
    `;

    document.body.appendChild(notification);

    setTimeout(() => {
        notification.style.transform = 'translateX(0)';
    }, 100);

    setTimeout(() => {
        notification.style.transform = 'translateX(100%)';
        setTimeout(() => {
            document.body.removeChild(notification);
        }, 300);
    }, 3000);
}

statusFilter.addEventListener('change', function () {
    const selectedStatus = this.value.toLowerCase(); // "" for all
    const articles = articlesContainer.querySelectorAll('.article-card');

    let visibleCount = 0;

    articles.forEach(article => {
        const status = article.getAttribute('data-status');
        const matchesStatus = selectedStatus === "" || status === selectedStatus;

        if (matchesStatus) {
            article.classList.remove('hidden');
            article.classList.add('filtered-in');
            article.classList.remove('filtered-out');
            visibleCount++;
        } else {
            article.classList.add('filtered-out');
            article.classList.remove('filtered-in');
            setTimeout(() => {
                article.classList.add('hidden');
            }, 300);
        }
    });

    // Optionally update empty state
    updateEmptyState(visibleCount);
});


