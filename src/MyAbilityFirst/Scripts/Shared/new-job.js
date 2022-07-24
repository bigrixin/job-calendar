jQuery(function ($) {
	$(document).ready(function () {

		$('#previewbtn').click(function () {
			if (!validationByDay()) {
				var question = "Some months have fewer than " + $('#day-monthly').val() + " days. For these months, the occurrence will fall onthe last day of the month.";
				confirm(question);
			}
			setPreviewData();
		});

		$('#submitcheck,#previewsubmit').click(function () {
			var startTime, endTime;
			if ($('#recurring-switch').prop('checked')) {
				if ($('#frequency-selecting').val() === "Weekly") {
					var selected = false;
					$('#dayofweek-container').find('input:checked').each(function () {
						selected = true;
					});
					if (!selected) {
						$('#dayofweekvalidation').html("Please select at least one option !");
						return false;
					}
				}
				else if ($('#frequency-selecting').val() === "Monthly") {
					if (!validationByDay()) {
						var question = "Some months have fewer than " + $('#day-monthly').val() + " days. For these months, the occurrence will fall onthe last day of the month.";
						return confirm(question);
					}
				}
			}

			if (!($('#Address_Suburb').val() && $('#Address_Postcode').val() && $('#Address_Latitude').val())) {
				$("#full-address").val('');
				$('#full-address').focus();
				$('#addressvalidation').html("This field is required !");
				$('#address-selecting').prop('checked', false);
				return false;
			}
			else
				$('#addressvalidation').html("");
		});

		$("#full-address").focusout(function () {
			setInterval(delayTimer, 200);
			function delayTimer() {
				document.getElementById("pre-address").innerText = $("#full-address").val();
				enablePreviewButton();
			}
		});

		$('#job-title').focus();

	});

	$('#submitcheck,#previewsubmit,#previewbtn,#previewbtn,#calendarbtn').click(function () {
		var startTime, endTime = null;
		if (getCurrentEvents().length === 0) {
			alert("No event be generated! please change end date !");
			$("#previewbtn").prop('disabled', true);
			$("#calendarbtn").prop('disabled', true);
			return false;
		}

		if ($('#recurring-switch').prop('checked')) {
			var dateString = moment($('#timepicker-jobstart-recurring').data("DateTimePicker").date()).format("YYYY-MM-DD");
			var startTimeStr = moment($('#timepicker-jobstart-recurring').data("DateTimePicker").date()).format("HH:mm");
			var endTimeStr = moment($('#timepicker-jobend-recurring').data("DateTimePicker").date()).format("HH:mm");
			startTime = mergeDatetime(dateString, startTimeStr);
			endTime = mergeDatetime(dateString, endTimeStr);
			if ($('#RecurringStartDate').val() === "" || $('#RecurringStartTime').val() === "" || $('#RecurringEndTime').val() === "") {
				alert("The Date and Time value can not be empty! ");
				return false;
			}
		}
		else {
			if ($('#StartDate').val() === "" || $('#StartTime').val() === "" || $('#EndTime').val() === "") {
				alert("The Date and Time value can not be empty! ");
				return false;
			}
			var dateString = moment($('#timepicker-jobstart').data("DateTimePicker").date()).format("YYYY-MM-DD");
			var startTimeStr = moment($('#timepicker-jobstart').data("DateTimePicker").date()).format("HH:mm");
			var endTimeStr = moment($('#timepicker-jobend').data("DateTimePicker").date()).format("HH:mm");
			startTime = mergeDatetime(dateString, $('StartTime').val());
			endTime = mergeDatetime(dateString, $('EndTime').val());
		}

		if (moment(startTime).isAfter(moment(endTime))) {
			alert("The DateTime value for 'start' must be less than 'end'.")
			return false;
		}

	});

	function mergeDatetime(dateString, timeString) {
		var dateTimeString = dateString + " " + timeString;
		return moment(dateTimeString, "YYYY-MM-DD HH:mm");
	}

	function validationByDay() {
		if ($('#frequency-monthly').children("option:selected").text() === "Day" && parseInt($('#day-monthly').val()) > 28) {
			var startDateTime = $("#datepicker-jobstart-recurring").data("DateTimePicker").date();
			var alert = false;
			var endday = "";
			$('#enddate-container').find('input[type=radio]:checked').each(function () {
				if (this.checked && this.value === 'endby') {
					var endDateTime = $("#timepicker-jobend-recurring").data("DateTimePicker").date();
					if (endDateTime.diff(startDateTime, 'days') > 30)
						alert = true;
				}
				else if (this.checked && this.value === 'after') {
					if (parseInt($('sequence-monthly').val()) >= 2 || parseInt($('#occurrence-recurring').val()) > 1)
						alert = true;
				}
				else
					alert = true;
			});
			if (alert)
				return false;
		}
		return true;
	}

	function setPreviewData() {
		document.getElementById("pre-name").innerText = getSelectedPatientName();
		document.getElementById("pre-title").innerText = $("#job-title").val();
		document.getElementById("pre-description").innerText = $("#job-description").val();
		document.getElementById("pre-address").innerText = $("#full-address").val();

		var gender = document.getElementById("gender-preference");
		document.getElementById("pre-gender-preference").innerText = gender.options[gender.selectedIndex].text;

		document.getElementById("pre-start-time").innerText = "";
		document.getElementById("pre-end-time").innerText = "";
		if ($('#recurring-switch').prop('checked')) {
			//recurring
			var recurringstartdate = $("#datepicker-jobstart-recurring").data("DateTimePicker").date();
			if (recurringstartdate !== null)
				document.getElementById("pre-start-date").innerText = recurringstartdate.format("dddd DD MMM YYYY");

			var starttime = $("#timepicker-jobstart-recurring").data("DateTimePicker").date();

			if (starttime !== null)
				document.getElementById("pre-start-time").innerText = starttime.format("hh:mm a");

			var endtime = $("#timepicker-jobend-recurring").data("DateTimePicker").date();
			if (endtime !== null)
				document.getElementById("pre-end-time").innerText = endtime.format("hh:mm a");

			var recurringfrequency = $('#frequency-selecting').val();
			if (recurringfrequency === 'Weekly') {
				var days = "";
				$('#dayofweek-container').find('input:checked').each(function () {
					var value = this.title.toString();
					if (days === "")
						days = days + value;
					else
						days = days + ", " + value;

				});
				recurringfrequency = recurringfrequency + ' recurring: Every ' + $("#sequence-weekly").val() + ' weeks on ' + days;
			}
			else {
				var frequencymonth = $("#frequency-monthly").children("option:selected").text();
				var dayofweek = $("#dayofweek-monthly").children("option:selected").text();
				if (frequencymonth === "Day")
					recurringfrequency = recurringfrequency + ' recurring: Every ' + $("#sequence-monthly").val() + ' months on ' + frequencymonth + ' ' + $("#day-monthly").val();
				else
					recurringfrequency = recurringfrequency + ' recurring: Every ' + $("#sequence-monthly").val() + ' months on ' + frequencymonth + ' ' + dayofweek;
			}

			var endday = "";
			$('#enddate-container').find('input[type=radio]:checked').each(function () {
				if (this.checked && this.value === 'endby') {
					endday = ", the end date is on " + $("#datepicker-jobend-recurring").data("DateTimePicker").date().format("dddd DD MMM YYYY");
				}
				else if (this.checked && this.value === 'after') {
					endday = ", the end date is after " + $('#occurrence-recurring').val() + " occurrences.";
				}
				else
					endday = ", no end date";
			});

			document.getElementById("pre-recurring").innerText = recurringfrequency + endday;
		}
		else {
			var norecurringstartdate = $("#datepicker-jobdate").data("DateTimePicker").date();
			if (norecurringstartdate !== null)
				document.getElementById("pre-start-date").innerText = norecurringstartdate.format("dddd DD MMM YYYY");

			var startTime = $("#timepicker-jobstart").data("DateTimePicker").date();
			if (startTime !== null)
				document.getElementById("pre-start-time").innerText = startTime.format("hh:mm a");


			var endTime = $("#timepicker-jobend").data("DateTimePicker").date();
			if (endTime !== null)
				document.getElementById("pre-end-time").innerText = endTime.format("hh:mm a");

		}
	}

	function getPatientInfo(id) {
		var url = $('#request-url').val();
		$("#patientinfo").empty();
		$.ajax({
			type: 'GET',
			url: url,
			dataType: 'json',
			data: {
				id: id
			},
			success: function (elements) {
				$.each(elements, function (i, element) {
					if ($('#address-selecting').prop('checked')) {
						document.getElementById(element.Text).value = element.Value;
					}
				});
			},
			error: function (ex) {
				alert('Failed to retrieve data' + ex);
			}
		});
		return false;
	}

	function getSelectedPatientID() {
		var id = 0;
		var count = 0;
		$('#patients-container input[type=checkbox]').each(function () {
			if (this.checked) {
				id = this.id.substr('patientid'.length);
				count++;
			}
		});
		if (count !== 1)
			id = 0;
		return id;
	}

	function getSelectedPatientName() {
		var names = '';
		index = 1;
		$('#patients-container input[type=checkbox]').each(function () {
			if (this.checked) {
				if (names !== '')
					names = names + ', ' + '(' + index.toString() + ') ';
				else
					names = '(' + index.toString() + ') ' + names;
				names = names + this.labels[0].innerText;
				index++;
			}

		});
		return names;
	}

	function getSelectedPatientCount() {
		var count = 0;
		$('#patients-container input[type=checkbox]').each(function () {
			if (this.checked) {
				count++;
			}
		});
		return count;
	}

	function enablePreviewButton() {
		var patientscount = getSelectedPatientCount();
		if ($("#job-title").val() && $("#job-description").val() && $("#full-address").val() && patientscount !== 0) {
			$('#dayofweekvalidation').html("");
			if ($("#recurring-switch").prop('checked')) {
				$("#previewbtn").prop('disabled', true);
				var enablePreview = true;

				if ($('#frequency-selecting').val() === "Weekly") {
					var selected = false;
					$('#dayofweek-container').find('input:checked').each(function () {
						selected = true;
					});
					if (!selected) {
						enablePreview = false;
						$("#previewbtn").prop('disabled', true);
						$('#dayofweekvalidation').html("Please select at least one option !");
					}
				}

				if ($("#datepicker-jobstart-recurring").data("DateTimePicker").date() === null)
					enablePreview = false;

				$('#enddate-container').find('input[type=radio]:checked').each(function () {
					if (this.checked && this.value === 'endby') {
						if ($("#datepicker-jobend-recurring").data("DateTimePicker").date() === null)
							enablePreview = false;
					}
				});

				if (enablePreview)
					$("#previewbtn").removeAttr("disabled");
			}
			else {
				if ($("#datepicker-jobdate").data("DateTimePicker").date() !== null)
					$("#previewbtn").removeAttr("disabled");
				else
					$("#previewbtn").prop('disabled', true);
			}
		}
		else {
			$("#previewbtn").prop('disabled', true);
		}
		if ($("#previewbtn").is(':disabled'))
			$("#calendarbtn").prop('disabled', true);
		else
			$("#calendarbtn").removeAttr("disabled");
	}

	//button, checkedbox, dropdownlist changed
	$(function () {
		//check selected patients
		$('#patients-container').change(function () {
			var patients = [];
			var selectpatients = 0;
			$('#patients-container input[type=checkbox]').each(function () {
				if (this.checked) {
					var id = this.id.substr('Patientid'.length);
					selectpatients++;
					patients.push(id);
					$('#PatientId').val(id);
				}
			});

			$('#PatientIDs').val(patients);

			if (selectpatients === 1) {
				$('.same-patient-profile').show();
			}
			else {
				$('.same-patient-profile').hide();
				if ($('#address-selecting').prop('checked'))
					$('#address-selecting').click();
			}

			if (patients.length > 0)
				$('#patientvalidation').html("");
			else
				$('#patientvalidation').html("Please select at least one option !");
			enablePreviewButton();
			document.getElementById('job-title').focus();
			document.getElementById('job-title').click();
		});

		//check selected day of week
		$('#dayofweek-container').change(function () {
			var days = [];
			$('#dayofweek-container input[type=checkbox]').each(function () {
				if (this.checked) {
					var id = this.id.substr('dayofweek'.length);
					days.push(id);
				}
			});
			$('#DayofWeeks').val(days);
			if (days.length > 0)
				$('#dayofweekvalidation').html("");
			else
				$('#dayofweekvalidation').html("Please select at least one option !");
			enablePreviewButton();
		});

		$('#recurring-switch').change(function () {
			if ($(this).prop('checked')) {
				$('.recurring-monthly').collapse('show');
				$('.no-recurring,.recurring-weekly').collapse('hide');
				$('.job-startdate,.job-enddate,.job-starttime,.job-endtime').val("");
				$('#RecurringStartDate').prop('required', true);
				$('#RecurringStartTime').prop('required', true);
				$('#RecurringEndTime').prop('required', true);
				$('#StartDate').removeAttr('required');
				$('#StartTime').removeAttr('required');
				$('#EndTime').removeAttr('required');
				$('#DayofWeeks').prop('required', true);
				$('#RecurringEndDate').prop('required', true);
				for (var i = 1; i < 7; i++) {
					$("#dayofweek" + i).prop("checked", false);
				}
			}
			else {
				$('.no-recurring,.recurring-weekly').collapse('show');
				$('.recurring-monthly').collapse('hide');
				$('#StartDate').prop('required', true);
				$('#StartTime').prop('required', true);
				$('#EndTime').prop('required', true);
				$('#RecurringStartDate').removeAttr('required');
				$('#RecurringStartTime').removeAttr('required');
				$('#RecurringEndTime').removeAttr('required');
				$('#DayofWeeks').removeAttr('required');
				$('#RecurringEndDate').removeAttr('required');
			}
			window.setTimeout(cleardatetime, 200);
			enablePreviewButton();
		});

		$('#frequency-selecting').change(function () {
			var selectValue = $(this).prop('value');
			if (selectValue === "Weekly") {
				$('.frequency-weekly').collapse('show');
				$('.frequency-monthly').collapse('hide');
				$('#DayofWeeks').prop('required', true);
			}
			else {
				$('.frequency-monthly').collapse('show');
				$('.frequency-weekly').collapse('hide');
				$('#DayofWeeks').removeAttr('required');
			}
		});

		$('#address-selecting').change(function () {
			$("#full-address").val('');
			$("#Address_Suburb").val('');
			$("#Address_State").val('');
			$("#Address_Postcode").val('');
			$("#Address_Latitude").val('');
			$("#Address_Longitude").val('');
			if ($(this).prop('checked')) {
				var patientid = getSelectedPatientID();
				if (patientid !== 0)
					window.setTimeout(getPatientInfo(patientid), 300);
			}
			enablePreviewButton();
		});

		//job frequency of monthly
		$('#frequency-monthly').change(function () {
			if (this.Text === "Day")
				$('#day-monthly').prop('max', 4);
			else
				$('#day-monthly').prop('max', 30);
			$('#day-monthly').val('1');

			var selectedday = $('#frequency-monthly').children("option:selected").text();
			if (selectedday === "Day") {
				$('#day-monthly').show();
				$('#dayofweek-monthly').hide();
			}
			else {
				$('#day-monthly').hide();
				$('#dayofweek-monthly').show();
			}
		});

		$('#enddate-container input[type=radio]').change(function () {
			switch (this.id) {
				case "enddate-none":
					$("#datepicker-jobend-recurring").hide();
					$("#occurrence").hide();
					$('#RecurringEndDate').removeAttr('required');
					break;
				case "enddate-after":

					$("#datepicker-jobend-recurring").hide();
					$("#occurrence").show();
					$('#RecurringEndDate').removeAttr('required');
					break;
				case "enddate-by":
					$("#datepicker-jobend-recurring").show();
					$("#occurrence").hide();
					$('#RecurringEndDate').prop('required', true);
					break;
			}
			$('#enddatevalidation').html("");
			var startDateTime = $("#datepicker-jobstart-recurring").data("DateTimePicker").date();
			if (startDateTime !== null)
				$('#datepicker-jobstart-recurring').data("DateTimePicker").maxDate(getMaxEndDate(startDateTime));
			enablePreviewButton();
		});

		$("#job-title, #job-description").focusout(function () {
			enablePreviewButton();
		});

		function cleardatetime() {
			$('#datepicker-jobdate').data("DateTimePicker").clear();
			$('#timepicker-jobstart').data("DateTimePicker").clear();
			$('#timepicker-jobend').data("DateTimePicker").clear();
			$("#datepicker-jobstart-recurring").data("DateTimePicker").clear();
			$("#datepicker-jobend-recurring").data("DateTimePicker").clear();
			$("#timepicker-jobstart-recurring").data("DateTimePicker").clear();
			$("#timepicker-jobend-recurring").data("DateTimePicker").clear();
		}
	});

	//date-time picker
	$(function () {

		//date picker
		$('#datepicker-jobdate,#datepicker-jobstart-recurring,#datepicker-jobend-recurring').datetimepicker({
			format: "DD/MM/YYYY",
			showTodayButton: true,
			showClose: true,
			showClear: true,
			toolbarPlacement: 'top',
			minDate: moment().hour(-1)
		});

		//time picker
		$('#timepicker-jobstart, #timepicker-jobend, #timepicker-jobstart-recurring, #timepicker-jobend-recurring').datetimepicker({
			format: "hh:mm A",
			showClose: true,
			showClear: true,
			minDate: moment({ h: 1, m: 10 }),
			maxDate: moment({ h: 23, m: 55 }),
			stepping: 10
		});

		//no recurring
		$("#datepicker-jobdate").on("dp.change", function (e) {
			if (e.date) {
				var startTime = e.date.format();
				var minStartTime = getMinStartTime(startTime);
				var maxStartTime = getMaxStartTime(startTime);
				var currentMinTime = $("#timepicker-jobstart").data("DateTimePicker").minDate();
				$('#timepicker-jobstart').data("DateTimePicker").minDate(false);
				$('#timepicker-jobstart').data("DateTimePicker").maxDate(false);
				$('#timepicker-jobstart').data("DateTimePicker").minDate(minStartTime);
				$('#timepicker-jobstart').data("DateTimePicker").maxDate(maxStartTime);
			}
			if ($('#timepicker-jobstart').data("DateTimePicker").date())
				$('#StartTime').attr("title", $('#timepicker-jobstart').data("DateTimePicker").date().format('DD/MM/YYYY HH:mm A'));
		});

		$('#timepicker-jobstart').on("dp.change", function (e) {
			if (e.date) {
				var startDate = $('#datepicker-jobdate').data("DateTimePicker").date();
				var startTime = $('#timepicker-jobstart').data("DateTimePicker").date();
				if (startDate === null)
					startDate = moment(e.date).add(moment.duration({ 'day': 1 })).format();

				var minTime = getMinStartTime(startDate);
				var maxTime = getMaxStartTime(startDate);
				$('#timepicker-jobend').data("DateTimePicker").maxDate(false);
				$('#timepicker-jobend').data("DateTimePicker").minDate(false);
				var maxEndTime = getMaxEndTime(startTime);
				$('#timepicker-jobend').data("DateTimePicker").maxDate(maxEndTime);
				var minEndTime = getMinEndTime(startTime);
				$('#timepicker-jobend').data("DateTimePicker").minDate(minEndTime);
				$('#timepicker-jobstart').data("DateTimePicker").maxDate(maxTime);
				if ($('#timepicker-jobstart').data("DateTimePicker").date())
					$('#StartTime').attr("title", $('#timepicker-jobstart').data("DateTimePicker").date().format('DD/MM/YYYY HH:mm A'));
				if ($('#timepicker-jobend').data("DateTimePicker").date())
					$('#EndTime').attr("title", $('#timepicker-jobend').data("DateTimePicker").date().format('DD/MM/YYYY HH:mm A'));
				enablePreviewButton();
			}
		});

		$('#timepicker-jobend').on("dp.change", function (e) {
			if (e.date) {
				$('#EndTime').attr("title", moment(e.date).format('DD/MM/YYYY HH:mm A'));
			}
		});

		//recurring
		$("#datepicker-jobstart-recurring").on("dp.change", function (e) {
			if (e.date) {
				var startTime = e.date.format();
				var minStartTime = getMinStartTime(startTime);
				var maxStartTime = getMaxStartTime(startTime);
				var currentMinTime = $("#timepicker-jobstart-recurring").data("DateTimePicker").minDate();
				$('#timepicker-jobstart-recurring').data("DateTimePicker").minDate(false);
				$('#timepicker-jobstart-recurring').data("DateTimePicker").maxDate(false);
				$('#timepicker-jobstart-recurring').data("DateTimePicker").minDate(minStartTime);
				$('#timepicker-jobstart-recurring').data("DateTimePicker").maxDate(maxStartTime);

				$('#datepicker-jobend-recurring').data("DateTimePicker").minDate(false);
				$('#datepicker-jobend-recurring').data("DateTimePicker").maxDate(false);
				$('#datepicker-jobend-recurring').data("DateTimePicker").minDate(moment(startTime));
				var maxEndDate = getMaxEndDate(startTime);
				$('#datepicker-jobend-recurring').data("DateTimePicker").maxDate(maxEndDate);
			}
			if ($('#timepicker-jobstart-recurring').data("DateTimePicker").date())
				$('#RecurringStartTime').attr("title", $('#timepicker-jobstart-recurring').data("DateTimePicker").date().format('DD/MM/YYYY HH:mm A'));
			enablePreviewButton();
		});

		$('#timepicker-jobstart-recurring').on("dp.change", function (e) {
			if (e.date) {
				var startDate = $('#datepicker-jobstart-recurring').data("DateTimePicker").date();
				var startTime = $('#timepicker-jobstart-recurring').data("DateTimePicker").date();
				if (startDate === null)
					startDate = moment(e.date).add(moment.duration({ 'day': 1 })).format();

				var minTime = getMinStartTime(startDate);
				var maxTime = getMaxStartTime(startDate);
				$('#timepicker-jobend-recurring').data("DateTimePicker").maxDate(false);
				$('#timepicker-jobend-recurring').data("DateTimePicker").minDate(false);
				var maxEndTime = getMaxEndTime(startTime);
				$('#timepicker-jobend-recurring').data("DateTimePicker").maxDate(maxEndTime);
				var minEndTime = getMinEndTime(startTime);
				$('#timepicker-jobend-recurring').data("DateTimePicker").minDate(minEndTime);
				$('#timepicker-jobstart-recurring').data("DateTimePicker").maxDate(maxTime);
				if ($('#timepicker-jobstart-recurring').data("DateTimePicker").date())
					$('#RecurringStartTime').attr("title", $('#timepicker-jobstart-recurring').data("DateTimePicker").date().format('DD/MM/YYYY HH:mm A'));
				if ($('#timepicker-jobend-recurring').data("DateTimePicker").date())
					$('#RecurringEndTime').attr("title", $('#timepicker-jobend-recurring').data("DateTimePicker").date().format('DD/MM/YYYY HH:mm A'));
				enablePreviewButton();
			}
		});

		$("#timepicker-jobend-recurring").on("dp.change", function (e) {
			if (e.date) {
				$('#RecurringEndTime').attr("title", moment(e.date).format('DD/MM/YYYY HH:mm A'));
				enablePreviewButton();
			}
		});
	});

	window.getMinStartTime = function (inputedDateTime) {
		var indate = moment(inputedDateTime).format();
		var inputeddate = moment(indate).format('YYYYMMDD');
		var today = moment().format('YYYYMMDD');
		var duration = moment.duration({ 'hours': 0.5 });
		var minTime = moment().add(duration);
		if (inputeddate > today) {
			var endTime = moment(inputedDateTime);
			minTime = moment([endTime.year(), endTime.month(), endTime.date(), '01', '10']);
		}
		return minTime;
	};

	window.getMaxStartTime = function (inputedDateTime) {
		var endTime = moment(inputedDateTime);
		var maxTime = moment([endTime.year(), endTime.month(), endTime.date(), '23', '55']);
		if (inputedDateTime > maxTime)
			return inputedDateTime;
		return maxTime;
	};

	window.getMinEndTime = function (inputedDateTime) {
		var indate = moment(inputedDateTime).format();
		var inputeddate = moment(indate).format('YYYYMMDD');
		var duration = moment.duration({ 'hours': 0.5 });
		var endTime = moment(inputedDateTime).add(duration);
		return moment(endTime);
	};

	window.getMaxEndTime = function (inputedDateTime) {
		var minEndTime = getMinEndTime(inputedDateTime);
		var duration = moment.duration({ 'hours': 23 });
		var maxEndTime = moment(minEndTime).add(duration);
		return moment(maxEndTime);
	};

	window.getMaxEndDate = function (inputedDateTime) {
		var duration = moment.duration({ 'year': 1 });
		var endTime = moment(inputedDateTime).add(duration);
		return moment(endTime);
	};

});

