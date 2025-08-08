let currentArticleId = null;

// Helper: Capitalize status and category
function capitalize(word) {
    return word ? word.charAt(0).toUpperCase() + word.slice(1) : "";
}

// Load articles from database and display them
async function loadArticles() {
    try {
        const response = await fetch('/api/articles/all');
        if (!response.ok) throw new Error("Failed to load articles");

        const articles = await response.json();
        displayArticles(articles);
    } catch (err) {
        console.error("Error loading articles:", err);
        alert("Error loading articles: " + err.message);
    }
}

// Display articles in the list view
function displayArticles(articles) {
    const listView = document.getElementById("listView");

    if (articles.length === 0) {
        listView.innerHTML = '<div class="no-articles">No articles found.</div>';
        return;
    }

    listView.innerHTML = ''; // Clear existing content

    articles.forEach(article => {
        addArticleToList(article);
    });
}

// Add article to listView
function addArticleToList(article) {
    const createdDate = new Date(article.createdAt).toLocaleDateString();
    const html = `
        <div class="article-item" data-status="${article.status}" data-category="${article.category}">
            <div class="article-thumbnail">
                <img src="${article.imagePath || 'https://via.placeholder.com/80x80/584a5a/f0f1f3?text=Article'}" alt="Article thumbnail">
            </div>
            <div class="article-details">
                <div class="article-header">
                    <h4>${article.title}</h4>
                    <div class="article-badges">
                        <span class="badge status-${article.status}">${capitalize(article.status)}</span>
                        <span class="badge category-${article.category}">${capitalize(article.category)}</span>
                    </div>
                </div>
                <p class="article-excerpt">${article.excerpt}</p>
                <div class="article-meta">
                    <span><i class="fas fa-calendar"></i> ${createdDate}</span>
                    <span><i class="fas fa-eye"></i> 0 views</span>
                    <span><i class="fas fa-thumbs-up"></i> 0 likes</span>
                    <span><i class="fas fa-user"></i> ${article.author}</span>
                </div>
            </div>
            <div class="article-actions">
                <button class="action-btn view" onclick="viewArticle(${article.id})" title="View">
                    <i class="fas fa-eye"></i>
                </button>
                <button class="action-btn edit" onclick="openArticleModal(${article.id})" title="Edit">
                    <i class="fas fa-edit"></i>
                </button>
                <button class="action-btn delete" onclick="deleteArticle(${article.id})" title="Delete">
                    <i class="fas fa-trash"></i>
                </button>
                <button class="action-btn more" onclick="toggleMoreActions(${article.id})" title="More">
                    <i class="fas fa-ellipsis-v"></i>
                </button>
            </div>
        </div>
    `;
    document.getElementById("listView").insertAdjacentHTML("afterbegin", html);
}

// Load article data into the form for editing
async function loadArticleData(articleId) {
    try {
        const response = await fetch('/api/articles/all');
        if (!response.ok) throw new Error("Failed to load articles");

        const articles = await response.json();
        const article = articles.find(a => a.id === articleId);

        if (!article) throw new Error("Article not found");

        document.getElementById('articleTitle').value = article.title || "";
        document.getElementById('articleCategory').value = article.category || "";
        document.getElementById('articleExcerpt').value = article.excerpt || "";
        document.getElementById('articleContent').value = article.content || "";
        document.getElementById('articleTags').value = article.tags || "";
        document.getElementById('articleStatus').value = article.status || "";
    } catch (err) {
        alert("Error loading article: " + err.message);
    }
}

// Delete article function
async function deleteArticle(articleId) {
    if (!confirm('Are you sure you want to delete this article?')) {
        return;
    }

    try {
        const response = await fetch(`/api/articles/delete?id=${articleId}`, {
            method: 'POST'
        });

        if (!response.ok) throw new Error("Failed to delete article");

        // Remove from DOM
        const articleElement = document.querySelector(`.article-item button[onclick="deleteArticle(${articleId})"]`).closest('.article-item');
        articleElement.remove();

        alert('Article deleted successfully!');
    } catch (err) {
        alert("Error deleting article: " + err.message);
    }
}

// Modal functions - open article form
function openArticleModal(articleId = null) {
    currentArticleId = articleId;
    const modal = document.getElementById('articleModal');
    const title = document.getElementById('modalTitle');

    if (articleId) {
        title.textContent = 'Edit Article';
        // Load article data here
        loadArticleData(articleId);
    } else {
        title.textContent = 'Create New Article';
        document.getElementById('articleForm').reset();
    }

    modal.style.display = 'flex';
    document.body.style.overflow = 'hidden';
}

// close article window function
function closeArticleModal() {
    document.getElementById('articleModal').style.display = 'none';
    document.body.style.overflow = 'auto';
    currentArticleId = null;
}

//// Load article data into the form for editing
//async function loadArticleData(articleId) {
//    try {
//        const response = await fetch(`/Articles/GetArticle/${articleId}`);
//        if (!response.ok) throw new Error("Failed to load article");

//        const article = await response.json();

//        document.getElementById('articleTitle').value = article.title || "";
//        document.getElementById('articleCategory').value = article.category || "";
//        document.getElementById('articleExcerpt').value = article.excerpt || "";
//        document.getElementById('articleContent').value = article.content || "";
//        document.getElementById('articleTags').value = article.tags || "";
//        document.getElementById('articleStatus').value = article.status || "";
//    } catch (err) {
//        alert("Error loading article: " + err.message);
//    }
//}

// Add article to listView
//function addArticleToList(article) {
//    const html = `
//        <div class="article-item" data-status="${article.status}" data-category="${article.category}">
//            <div class="article-thumbnail">
//                <img src="https://via.placeholder.com/80x80?text=Article" alt="Article thumbnail">
//            </div>
//            <div class="article-details">
//                <div class="article-header">
//                    <h4>${article.title}</h4>
//                    <div class="article-badges">
//                        <span class="badge status-${article.status}">${capitalize(article.status)}</span>
//                        <span class="badge category-${article.category}">${capitalize(article.category)}</span>
//                    </div>
//                </div>
//                <p class="article-excerpt">${article.excerpt}</p>
//                <div class="article-meta">
//                    <span><i class="fas fa-calendar"></i> ${new Date(article.createdAt || Date.now()).toLocaleDateString()}</span>
//                    <span><i class="fas fa-eye"></i> 0 views</span>
//                    <span><i class="fas fa-thumbs-up"></i> 0 likes</span>
//                </div>
//            </div>
//            <div class="article-actions">
//                <button class="action-btn view" onclick="viewArticle(${article.id})" title="View"><i class="fas fa-eye"></i></button>
//                <button class="action-btn edit" onclick="openArticleModal(${article.id})" title="Edit"><i class="fas fa-edit"></i></button>
//                <button class="action-btn delete" onclick="deleteArticle(${article.id})" title="Delete"><i class="fas fa-trash"></i></button>
//            </div>
//        </div>
//    `;
//    document.getElementById("listView").insertAdjacentHTML("afterbegin", html);
//}

// Handle form submit
document.getElementById('articleForm').addEventListener('submit', async function (e) {
    e.preventDefault();

    const article = {
        id: currentArticleId || 0,
        title: document.getElementById('articleTitle').value,
        category: document.getElementById('articleCategory').value,
        excerpt: document.getElementById('articleExcerpt').value,
        content: document.getElementById('articleContent').value,
        tags: document.getElementById('articleTags').value,
        status: document.getElementById('articleStatus').value,
        author: "Jasmine", // Replace with dynamic if needed
        isApproved: false,
        adminComment: "",
        imagePath: "" // or a placeholder URL

    };



    try {
        const url = currentArticleId ? '/api/articles/update' : '/api/articles/create';
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(article)
        });

        if (!response.ok) throw new Error("Failed to save article");

        const savedArticle = await response.json();

        if (currentArticleId) {
            // Update existing article in DOM
            loadArticles(); // Reload all articles for simplicity
        } else {
            // Add new article to DOM
            addArticleToList(savedArticle);
        }

        closeArticleModal();
        alert(currentArticleId ? 'Article updated successfully!' : 'Article created successfully!');
    } catch (err) {
        alert("Error: " + err.message);
    }
});

// Load articles when page loads
document.addEventListener('DOMContentLoaded', function () {
    loadArticles();
});
