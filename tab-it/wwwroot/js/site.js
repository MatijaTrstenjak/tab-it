(() => {
	const sidebar = document.querySelector(".app-sidebar");
	const menuToggle = document.querySelector(".app-menu-toggle");
	const menu = document.getElementById("app-main-menu");

	if (sidebar && menuToggle && menu) {
		menuToggle.addEventListener("click", () => {
			const isOpen = sidebar.classList.toggle("is-open");
			menuToggle.setAttribute("aria-expanded", String(isOpen));
		});
	}

	const currentPath = window.location.pathname.toLowerCase();
	document.querySelectorAll(".app-menu a").forEach((link) => {
		const href = (link.getAttribute("href") || "").toLowerCase();
		if (href && (currentPath === href || currentPath.startsWith(`${href}/`))) {
			link.classList.add("is-active");
			link.setAttribute("aria-current", "page");
		}
	});
})();
