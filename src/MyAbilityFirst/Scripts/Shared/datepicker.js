jQuery(function ($) {

	$('input[type=datetime]').datepicker({
		changeMonth: true,
		changeYear: true,
		dateFormat: 'dd/mm/yyyy'

	});

	//for chrome and safari browsers
	jQuery.validator.methods.date = function (value, element) {
		var d = value.split("/");
		return this.optional(element) || !/Invalid|NaN/.test(new Date(/chrom(e|ium)/.test(navigator.userAgent.toLowerCase()) ? d[1] + "/" + d[0] + "/" + d[2] : value));
	};
});
