jQuery(function ($) {
	//Fix datetime picker conflict on chrome or safari
	jQuery.validator.methods.date = function (value, element) {
		var isChrome = /Chrome/.test(navigator.userAgent) && /Google Inc/.test(navigator.vendor);
		var isSafari = /Safari/.test(navigator.userAgent) && /Apple Computer/.test(navigator.vendor);
		if (isSafari || isChrome) {
			var d = value.split("/");
			return this.optional(element) || !/Invalid|NaN/.test(new Date(/chrom(e|ium)/.test(navigator.userAgent.toLowerCase()) ? d[1] + "/" + d[0] + "/" + d[2] : value));
		} else {
			return this.optional(element) || !/Invalid|NaN/.test(new Date(value));
		}
	};

	//date-time picker
	$(function () {
		$('#datepicker-birthday').datetimepicker({
			format: "DD/MM/YYYY",
			showClose: true,
			showClear: true,
			toolbarPlacement: 'top',
			maxDate: Date.now()
		});

		$('#birthday').val("");
	});

});

