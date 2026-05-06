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
$(document).ready(function () {
    // Bind to all buttons that should open the form inside a modal
    $(document).on('click', '.ajax-modal-link', function (e) {
        e.preventDefault();
        var url = $(this).attr('href');
        
        // try to get title from data-title attribute. If not, try to look for nearest h2
        var title = $(this).data('title');
        if (!title) {
            title = $(this).siblings('a').find('h2').text();
        }
        if (!title) {
            title = $(this).closest('section').find('h1').text();
        }
        
        $('#globalFormModalLabel').text("Create New " + title);
        $('#globalFormModalBody').html('<div class="text-center my-3"><div class="spinner-border text-primary" role="status"></div></div>');
        $('#globalFormModal').modal('show');
        
        // Fetch the form from the endpoint and inject it
        $('#globalFormModalBody').load(url + ' form', function(response, status, xhr) {
            if (status == "error") {
                $('#globalFormModalBody').html('<div class="alert alert-danger">Error loading form.</div>');
                return;
            }
            
            // Re-bind validation for dynamically loaded content limits 
            if ($.validator && $.validator.unobtrusive) {
                $("form").removeData("validator");
                $("form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("#globalFormModalBody form");
            }
        });
    });
});