document.addEventListener('DOMContentLoaded', function () {
    // Initialize functionality
    initializeSearch();
    initializeFiltering();
    initializeViewToggle();

    // Search functionality
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

    // Filter articles based on search and category
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

        // Show/hide empty state
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

    // View toggle functionality
    function initializeViewToggle() {
        const viewButtons = document.querySelectorAll('.view-btn');
        const articlesContainer = document.getElementById('articlesContainer');

        viewButtons.forEach(button => {
            button.addEventListener('click', function () {
                // Remove active class from all buttons
                viewButtons.forEach(btn => btn.classList.remove('active'));
                // Add active class to clicked button
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

    // Initialize filtering
    function initializeFiltering() {
        // Any additional filtering setup can go here
        console.log('Filtering initialized');
    }
});

// Article preview modal functionality
function previewArticle(title, content, category, author, date, adminComment) {
    const modal = document.getElementById('previewModal');
    const previewTitle = document.getElementById('previewTitle');
    const previewCategory = document.getElementById('previewCategory');
    const previewContent = document.getElementById('previewContent');
    const previewAdminSection = document.getElementById('previewAdminSection');
    const previewAdminComment = document.getElementById('previewAdminComment');

    // Set modal content
    previewTitle.textContent = title;
    previewCategory.textContent = category;
    previewContent.textContent = content;

    // Show/hide admin comment section
    if (adminComment && adminComment.trim() !== '' && adminComment !== 'null') {
        previewAdminComment.textContent = adminComment;
        previewAdminSection.style.display = 'block';
    } else {
        previewAdminSection.style.display = 'none';
    }

    // Show modal
    modal.style.display = 'block';
    document.body.style.overflow = 'hidden';

    // Add click outside to close
    modal.onclick = function (event) {
        if (event.target === modal) {
            closeModal();
        }
    }
}

function closeModal() {
    const modal = document.getElementById('previewModal');
    modal.style.display = 'none';
    document.body.style.overflow = 'auto';
}

// Keyboard shortcuts
document.addEventListener('keydown', function (event) {
    // Close modal with Escape key
    if (event.key === 'Escape') {
        closeModal();
    }

    // Focus search with Ctrl+F or Cmd+F
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

// Export article data (if needed)
function exportArticleData() {
    const articles = [];
    const articleCards = document.querySelectorAll('.article-card:not(.hidden)');

    articleCards.forEach(card => {
        const title = card.querySelector('.article-title').textContent;
        const category = card.getAttribute('data-category');
        const author = card.querySelector('.author').textContent;
        const date = card.querySelector('.date').textContent;

        articles.push({
            title,
            category,
            author,
            date
        });
    });

    console.log('Visible articles:', articles);
    return articles;
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
    // This function can be implemented if you have many articles
    // and need to improve performance with virtual scrolling
    console.log('List virtualization can be implemented here if needed');
}

// Analytics tracking (if needed)
function trackArticleView(articleTitle) {
    // Track when users preview articles
    console.log(`Article previewed: ${articleTitle}`);
    // You can integrate with your analytics service here
}

// Article sharing functionality (if needed)
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


