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
    function initDatePickers(scope) {
        var root = scope ? $(scope) : $(document);
        if (!window.flatpickr) {
            return;
        }

        root.find(".js-datetime").each(function () {
            if (this._flatpickr) {
                return;
            }

            var locale = (navigator.language || "en").toLowerCase();
            var isHr = locale.startsWith("hr");

            window.flatpickr(this, {
                enableTime: true,
                time_24hr: isHr,
                allowInput: true,
                dateFormat: "Y-m-d H:i",
                altInput: true,
                altFormat: isHr ? "d.m.Y H:i" : "m/d/Y h:i K",
                locale: isHr ? "hr" : "default"
            });
        });
    }

    function initAutocomplete(scope) {
        var root = scope ? $(scope) : $(document);
        root.find(".autocomplete-field").each(function () {
            var $field = $(this);
            if ($field.data("autocomplete-bound")) {
                return;
            }

            $field.data("autocomplete-bound", true);
            var $input = $field.find(".js-autocomplete-input");
            var $value = $field.find(".js-autocomplete-value");
            var $menu = $field.find(".autocomplete-menu");
            var searchUrl = $field.data("search-url");
            var timer = null;

            if (!searchUrl) {
                return;
            }

            function renderOptions(items) {
                $menu.empty();
                if (!items || items.length === 0) {
                    $menu.append('<div class="autocomplete-empty">No matches</div>');
                    $menu.addClass("is-open");
                    return;
                }

                items.forEach(function (item) {
                    var option = $("<button/>", {
                        type: "button",
                        class: "autocomplete-option",
                        text: item.text,
                        "data-id": item.id
                    });
                    $menu.append(option);
                });
                $menu.addClass("is-open");
            }

            $input.on("input", function () {
                var query = $input.val();
                $value.val("");

                if (timer) {
                    clearTimeout(timer);
                }

                timer = setTimeout(function () {
                    if (!query || query.trim().length < 1) {
                        $menu.removeClass("is-open").empty();
                        return;
                    }

                    $.getJSON(searchUrl, { q: query })
                        .done(renderOptions)
                        .fail(function () {
                            renderOptions([]);
                        });
                }, 220);
            });

            $menu.on("click", ".autocomplete-option", function () {
                var $option = $(this);
                $value.val($option.data("id"));
                $input.val($option.text());
                $menu.removeClass("is-open").empty();
            });

            $(document).on("click", function (event) {
                if (!$field.is(event.target) && $field.has(event.target).length === 0) {
                    $menu.removeClass("is-open");
                }
            });
        });
    }
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

            initDatePickers("#globalFormModalBody");
            initAutocomplete("#globalFormModalBody");
        });
    });

    function applyRowReveal($target) {
        $target.find("tr").addClass("row-reveal").each(function (index) {
            var row = this;
            setTimeout(function () {
                row.classList.add("row-reveal-in");
            }, 30 + index * 20);
        });
    }

    var searchTimers = {};
    $(document).on("input", ".js-live-search", function () {
        var $input = $(this);
        var targetSelector = $input.data("target");
        var searchUrl = $input.data("search-url");

        if (!targetSelector || !searchUrl) {
            return;
        }

        var key = searchUrl + targetSelector;
        if (searchTimers[key]) {
            clearTimeout(searchTimers[key]);
        }

        searchTimers[key] = setTimeout(function () {
            var query = $input.val();
            var $target = $(targetSelector);
            if ($target.length === 0) {
                return;
            }

            $target.addClass("is-loading");
            $.get(searchUrl, { q: query })
                .done(function (html) {
                    $target.fadeTo(120, 0.2, function () {
                        $target.html(html);
                        applyRowReveal($target);
                        $target.fadeTo(180, 1);
                    });
                })
                .always(function () {
                    $target.removeClass("is-loading");
                });
        }, 240);
    });

    initDatePickers();
    initAutocomplete();
});