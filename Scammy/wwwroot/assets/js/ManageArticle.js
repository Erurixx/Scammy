document.addEventListener('DOMContentLoaded', function () {
    const searchInput = document.getElementById('searchInput');
    const categoryFilter = document.getElementById('categoryFilter');
    const statusFilter = document.getElementById('statusFilter');
    const resetBtn = document.getElementById('resetFilters'); 
    const articleCards = document.querySelectorAll('.article-card');

    function filterArticles() {
        const searchTerm = searchInput.value.toLowerCase();
        const selectedCategory = categoryFilter.value;
        const selectedStatus = statusFilter.value.toLowerCase();

        let visibleCount = 0;

        articleCards.forEach(card => {
            const title = card.getAttribute('data-title').toLowerCase();
            const excerpt = card.getAttribute('data-excerpt').toLowerCase();
            const category = card.getAttribute('data-category');
            const status = card.getAttribute('data-status').toLowerCase();

            const matchesSearch = title.includes(searchTerm) || excerpt.includes(searchTerm);
            const matchesCategory = selectedCategory === '' || category === selectedCategory;
            const matchesStatus = selectedStatus === '' || status === selectedStatus;

            if (matchesSearch && matchesCategory && matchesStatus) {
                card.classList.remove('d-none');
                visibleCount++;
            } else {
                card.classList.add('d-none');
            }
        });

        let emptyState = document.querySelector('.empty-state');
        if (visibleCount === 0 && !emptyState) {
            emptyState = document.createElement('tr');
            emptyState.className = 'empty-state';
            emptyState.innerHTML = `<td colspan="6" class="text-center">No articles found</td>`;
            document.querySelector('tbody').appendChild(emptyState);
        } else if (visibleCount > 0 && emptyState) {
            emptyState.remove();
        }
    }

    searchInput.addEventListener('input', filterArticles);
    categoryFilter.addEventListener('change', filterArticles);
    statusFilter.addEventListener('change', filterArticles);

    // Reset 
    resetBtn.addEventListener('click', function () {
        searchInput.value = '';
        categoryFilter.selectedIndex = 0;
        statusFilter.selectedIndex = 0;
        filterArticles();
    });
    // Header reset
    let lastScrollTop = 0;
    let headerFixed = false;

    function resetHeader() {
        $('.header-area, .background-header').removeAttr('style');
        $('.header-area .main-nav').removeAttr('style');
        $('.header-area .main-nav .nav').removeAttr('style');
        $('.header-area .main-nav .nav li').removeAttr('style');
        $('.dropdown-menu, .submenu ul, .dropdown, .submenu').removeAttr('style');
    }

    $(window).scroll(function () {
        let scrollTop = $(this).scrollTop();
        if (scrollTop < 200 && !headerFixed) {
            resetHeader();
        } else if (scrollTop > 200) {
            headerFixed = false;
        }
        lastScrollTop = scrollTop;
    });

    resetHeader();
    $(document).on('click', function () {
        if (!headerFixed) setTimeout(resetHeader, 100);
    });

    setInterval(function () {
        if ($(window).scrollTop() < 200) resetHeader();
    }, 5000);
});


