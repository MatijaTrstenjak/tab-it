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

    function initImageDropzones(scope) {
        var root = scope ? $(scope) : $(document);
        root.find(".image-dropzone").each(function () {
            var dropzone = this;
            var $dropzone = $(dropzone);
            if ($dropzone.data("image-dropzone-bound")) {
                return;
            }

            $dropzone.data("image-dropzone-bound", true);
            var input = dropzone.querySelector('input[type="file"]');
            var preview = dropzone.querySelector("[data-image-preview]");

            if (!input || !preview) {
                return;
            }

            function showPreview(file) {
                if (!file || !file.type || !file.type.startsWith("image/")) {
                    return;
                }

                preview.src = URL.createObjectURL(file);
                preview.alt = file.name;
                preview.classList.remove("d-none");
            }

            input.addEventListener("change", function () {
                showPreview(input.files[0]);
            });

            ["dragenter", "dragover"].forEach(function (eventName) {
                dropzone.addEventListener(eventName, function (event) {
                    event.preventDefault();
                    dropzone.classList.add("dragging");
                });
            });

            ["dragleave", "drop"].forEach(function (eventName) {
                dropzone.addEventListener(eventName, function (event) {
                    event.preventDefault();
                    dropzone.classList.remove("dragging");
                });
            });

            dropzone.addEventListener("drop", function (event) {
                var file = event.dataTransfer.files[0];
                if (!file) {
                    return;
                }

                input.files = event.dataTransfer.files;
                showPreview(file);
            });
        });
    }

    function bindModalForm() {
        var $modalBody = $("#globalFormModalBody");
        var $modalForm = $modalBody.find("form");

        if (!$modalForm.length) {
            return;
        }

        if ($.validator && $.validator.unobtrusive) {
            $modalForm.removeData("validator");
            $modalForm.removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse($modalForm);
        }

        initDatePickers($modalBody);
        initAutocomplete($modalBody);
        initImageDropzones($modalBody);

        $modalForm.off("submit.modalForm").on("submit.modalForm", function (submitEvent) {
            submitEvent.preventDefault();

            if ($.validator && !$modalForm.valid()) {
                return;
            }

            var $submit = $modalForm.find('[type="submit"]');
            $submit.prop("disabled", true);

            var isMultipart = (($modalForm.attr("enctype") || "").toLowerCase() === "multipart/form-data");
            var ajaxOptions = {
                url: $modalForm.attr("action") || window.location.href,
                method: ($modalForm.attr("method") || "POST").toUpperCase(),
                data: isMultipart ? new FormData($modalForm[0]) : $modalForm.serialize(),
                headers: {
                    "X-Requested-With": "XMLHttpRequest"
                }
            };

            if (isMultipart) {
                ajaxOptions.processData = false;
                ajaxOptions.contentType = false;
            }

            $.ajax(ajaxOptions)
                .done(function (html) {
                    var $response = $("<div>").append($.parseHTML(html, document, true));
                    var $returnedForm = $response.find("main form").first();
                    if (!$returnedForm.length) {
                        $returnedForm = $response.find(".app-main-content form").first();
                    }
                    if (!$returnedForm.length) {
                        $returnedForm = $response.find("form").not('[action*="Logout"]').first();
                    }

                    if ($returnedForm.length) {
                        $modalBody.html($returnedForm);
                        polishModalContent(currentModalMode());
                        bindModalForm();
                        return;
                    }

                    window.location.reload();
                })
                .fail(function (xhr) {
                    var $response = $("<div>").append($.parseHTML(xhr.responseText || "", document, true));
                    var $returnedForm = $response.find("main form").first();
                    if (!$returnedForm.length) {
                        $returnedForm = $response.find(".app-main-content form").first();
                    }
                    if (!$returnedForm.length) {
                        $returnedForm = $response.find("form").not('[action*="Logout"]').first();
                    }

                    if ($returnedForm.length) {
                        $modalBody.html($returnedForm);
                        polishModalContent(currentModalMode());
                        bindModalForm();
                        return;
                    }

                    $modalBody.prepend('<div class="alert alert-danger">Save failed. Check the entered values and try again.</div>');
                })
                .always(function () {
                    $submit.prop("disabled", false);
                });
        });
    }

    function modalModeFromUrl(url) {
        if (/details(\/|\?|$)/i.test(url)) {
            return "details";
        }

        if (/\/edit(\/|\?|$)/i.test(url)) {
            return "edit";
        }

        if (/\/delete(\/|\?|$)/i.test(url)) {
            return "delete";
        }

        return "add";
    }

    function actionTitleFromMode(mode) {
        if (mode === "details") {
            return "Details: ";
        }

        if (mode === "edit") {
            return "Edit ";
        }

        if (mode === "delete") {
            return "Delete ";
        }

        return "Add ";
    }

    function currentModalMode() {
        var $modal = $("#globalFormModal");
        if ($modal.hasClass("modal-mode-delete")) {
            return "delete";
        }

        if ($modal.hasClass("modal-mode-edit")) {
            return "edit";
        }

        if ($modal.hasClass("modal-mode-details")) {
            return "details";
        }

        return "add";
    }

    function polishModalContent(mode) {
        var $modal = $("#globalFormModal");
        var $modalBody = $("#globalFormModalBody");

        $modal
            .removeClass("modal-mode-add modal-mode-edit modal-mode-delete modal-mode-details")
            .addClass("modal-mode-" + mode);

        $modalBody.find("form").addClass(mode === "delete" ? "modal-delete-form" : "modal-crud-form");
        $modalBody.find(".app-panel").addClass("modal-panel");

        $modalBody.find('a[href]').each(function () {
            var $link = $(this);
            var href = ($link.attr("href") || "").toLowerCase();
            var text = $link.text().trim().toLowerCase();

            if (href.endsWith("/index") || text === "back to list" || text === "cancel") {
                $link.attr("href", "#");
                $link.attr("data-bs-dismiss", "modal");
                $link.addClass("modal-cancel-link");
                if (mode === "delete") {
                    $link.text("Keep item");
                } else if (text === "back to list") {
                    $link.text("Cancel");
                }
            }
        });
    }

    function loadModalContent(url, titlePrefix, title) {
        var mode = modalModeFromUrl(url);
        $('#globalFormModalLabel').text((titlePrefix || "") + (title || ""));
        $('#globalFormModalBody').html('<div class="text-center my-3"><div class="spinner-border text-primary" role="status"></div></div>');
        $("#globalFormModal")
            .removeClass("modal-mode-add modal-mode-edit modal-mode-delete modal-mode-details")
            .addClass("modal-mode-" + mode);
        $('#globalFormModal').modal('show');

        $.get(url)
            .done(function (response) {
                var $response = $("<div>").append($.parseHTML(response, document, true));
                var $content = $response.find(".app-panel").first();

                if (!$content.length) {
                    $content = $response.find("main form").first();
                }

                if (!$content.length) {
                    $content = $response.find(".app-main-content form").first();
                }

                if (!$content.length) {
                    $content = $response.find("form").not('[action*="Logout"]').first();
                }

                if (!$content.length) {
                    $content = $response.find("main").first();
                }

                $('#globalFormModalBody').html($content.length ? $content : response);
                polishModalContent(mode);
                bindModalForm();
            })
            .fail(function () {
                $('#globalFormModalBody').html('<div class="alert alert-danger">Error loading content.</div>');
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
        
        var action = actionTitleFromMode(modalModeFromUrl(url));

        loadModalContent(url, action, title);
    });

    $(document).on("click", ".ajax-row-link", function (e) {
        if ($(e.target).closest("a, button, input, select, textarea, label").length) {
            return;
        }

        var url = $(this).data("href");
        if (!url) {
            return;
        }

        var title = $(this).data("title") || "Item";
        loadModalContent(url, "Details: ", title);
    });

    $(document).on("click", "#globalFormModalBody a[href]", function (e) {
        var link = this;
        var href = link.getAttribute("href") || "";

        if (!/details(\/|\?|$)/i.test(href)) {
            return;
        }

        e.preventDefault();

        var label = $(link).text().trim() || "Item";
        loadModalContent(href, "Details: ", label);
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
    initImageDropzones();
});
