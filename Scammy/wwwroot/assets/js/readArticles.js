// ================== Progress Bar ==================
window.addEventListener("scroll", () => {
    const scrollTop = window.scrollY;
    const docHeight = document.documentElement.scrollHeight - window.innerHeight;
    const progress = (scrollTop / docHeight) * 100;
    document.getElementById("progressBar").style.width = progress + "%";
});

// ================== Table of Contents ==================
document.addEventListener("DOMContentLoaded", () => {
    const relatedNav = document.getElementById("tocNav");

    // Parse JSON from data attribute
    const relatedArticles = JSON.parse(relatedNav.dataset.articles);

    relatedArticles.forEach(article => {
        const link = document.createElement("a");
        link.href = `/Home/readArticles/${article.Id}?slug=${article.Title.replace(/\s+/g, '-').toLowerCase()}`;
        link.textContent = article.Title;
        relatedNav.appendChild(link);
    });

    const tocToggle = document.getElementById("tocToggle");
    tocToggle.addEventListener("click", () => {
        relatedNav.classList.toggle("hidden");
        tocToggle.querySelector("i").classList.toggle("fa-chevron-up");
        tocToggle.querySelector("i").classList.toggle("fa-chevron-down");
    });
});



// ================== Share ==================
const shareBtn = document.getElementById("shareBtn");
if (shareBtn) {
    shareBtn.addEventListener("click", async () => {
        const title = document.querySelector(".article-title").textContent;
        const url = window.location.href;

        if (navigator.share) {
            try {
                await navigator.share({ title, url });
            } catch (err) {
                console.log("Share canceled", err);
            }
        } else {
            navigator.clipboard.writeText(url);
            alert("Link copied to clipboard!");
        }
    });
}

// ================== Font Size & Font Family ==================
const fontSizeBtn = document.getElementById("fontSizeBtn");
const fontSizeModal = document.getElementById("fontSizeModal");
const articleBody = document.querySelector(".article-body");

if (fontSizeBtn && fontSizeModal) {
    fontSizeBtn.addEventListener("click", () => {
        fontSizeModal.style.display = "block";
    });
}

function closeFontModal() {
    fontSizeModal.style.display = "none";
}
window.closeFontModal = closeFontModal;

document.querySelectorAll(".size-btn").forEach(btn => {
    btn.addEventListener("click", () => {
        document.querySelectorAll(".size-btn").forEach(b => b.classList.remove("active"));
        btn.classList.add("active");

        switch (btn.dataset.size) {
            case "small":
                articleBody.style.fontSize = "14px";
                break;
            case "medium":
                articleBody.style.fontSize = "16px";
                break;
            case "large":
                articleBody.style.fontSize = "18px";
                break;
            case "extra-large":
                articleBody.style.fontSize = "20px";
                break;
        }
    });
});

document.getElementById("fontFamilySelect").addEventListener("change", e => {
    switch (e.target.value) {
        case "georgia":
            articleBody.style.fontFamily = "Georgia, serif";
            break;
        case "times":
            articleBody.style.fontFamily = "'Times New Roman', serif";
            break;
        case "arial":
            articleBody.style.fontFamily = "Arial, sans-serif";
            break;
        default:
            articleBody.style.fontFamily = "'Crimson Text', serif";
    }
});

// ================== Dark Mode ==================
const darkModeBtn = document.getElementById("darkModeBtn");
if (darkModeBtn) {
    darkModeBtn.addEventListener("click", () => {
        document.body.classList.toggle("dark-mode");
        darkModeBtn.querySelector("i").classList.toggle("fa-moon");
        darkModeBtn.querySelector("i").classList.toggle("fa-sun");
    });
}

// ================== Focus Mode ==================
const focusModeBtn = document.getElementById("focusModeBtn");
if (focusModeBtn) {
    focusModeBtn.addEventListener("click", () => {
        document.querySelector(".table-of-contents").classList.toggle("hidden");
        document.querySelector(".reading-tools").classList.toggle("hidden");
    });
}
