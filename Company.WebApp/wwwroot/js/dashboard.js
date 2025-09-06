// dashboard.js
(() => {
    // Helper: safe query
    const $ = (s, root = document) => root.querySelector(s);

    // Page enter animation: add classes then remove after animation ends
    document.documentElement.classList.add('page-enter');
    requestAnimationFrame(() => {
        document.documentElement.classList.add('page-enter-active');
    });
    // remove enter classes after animation to keep DOM clean
    setTimeout(() => {
        document.documentElement.classList.remove('page-enter', 'page-enter-active');
    }, 600);

    // NAVBAR scroll effect
    function initNavbarScroll() {
        const navbar = document.querySelector('.admin-main-container .navbar') || document.querySelector('.navbar');
        if (!navbar) return;
        const onScroll = () => {
            if (window.scrollY > 50) navbar.classList.add('scrolled');
            else navbar.classList.remove('scrolled');
        };
        window.addEventListener('scroll', onScroll, { passive: true });
        onScroll();
    }

    // RIPPLE + page-exit for links with data-transition="true"
    function initRippleAndTransitions() {
        document.addEventListener('click', function (ev) {
            const target = ev.target;
            // find closest actionable element
            const el = target.closest('.nav-link, .btn, .logout, [data-ripple]');
            if (!el) return;

            // --- RIPPLE ---
            // ensure element can contain absolute children
            const style = window.getComputedStyle(el);
            if (style.position === 'static') {
                // mark that we changed it so we can clean later (optional)
                el.dataset._savedPosition = el.style.position || '';
                el.style.position = 'relative';
            }
            if (style.overflow !== 'hidden') {
                // keep overflow hidden for ripple
                el.dataset._savedOverflow = el.style.overflow || '';
                el.style.overflow = 'hidden';
            }

            const rect = el.getBoundingClientRect();
            const circle = document.createElement('span');
            circle.className = 'ripple';
            const size = Math.max(rect.width, rect.height);
            circle.style.width = circle.style.height = size + 'px';
            // position circle centered on click
            circle.style.left = (ev.clientX - rect.left - size / 2) + 'px';
            circle.style.top = (ev.clientY - rect.top - size / 2) + 'px';

            el.appendChild(circle);
            setTimeout(() => {
                circle.remove();
                // restore original inline styles if we saved them
                if (el.dataset._savedPosition !== undefined) {
                    el.style.position = el.dataset._savedPosition;
                    delete el.dataset._savedPosition;
                }
                if (el.dataset._savedOverflow !== undefined) {
                    el.style.overflow = el.dataset._savedOverflow;
                    delete el.dataset._savedOverflow;
                }
            }, 650);

            // --- PAGE TRANSITION (only for anchors with data-transition="true") ---
            const anchor = el.closest('a[href]');
            if (anchor && anchor.dataset.transition === "true") {
                // prevent immediate navigation
                ev.preventDefault();
                // play exit animation
                document.documentElement.classList.add('page-exit-active');

                // after animation, navigate (uses location.href to be safe)
                const href = anchor.href;
                setTimeout(() => {
                    // if it's the same-origin internal link, still do full navigate:
                    window.location.href = href;
                }, 360); // match CSS transition duration
            }
        }, false);
    }

    // Initialize everything when DOM is ready
    function init() {
        initNavbarScroll();
        initRippleAndTransitions();
        // in case Blazor re-renders DOM later, we still rely on event delegation so no re-init needed
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
