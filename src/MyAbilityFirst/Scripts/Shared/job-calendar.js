jQuery(function ($) {
	//Fix conflict on chrome or safari
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

	var overLapWarning = true;
	var currentEventsSource = null;
	var existingEventsSource = null;
	var jsonDataSource = null;
	var existingScheduleID = "existing-schedule";
	var calendarname = "abilityfirst-jobs-preview";
	var cal = ics(calendarname);
	var eventsdatearray = [];
	localStorage.removeItem('ExitingSchedules');

	$('#calendar').fullCalendar({
		header: {
			left: 'prev, next today',
			center: 'title',
			right: 'month,agendaWeek,agendaDay,listYear'
		},
		eventColor: '#69a33f',
		timezone: 'local',
		slotEventOverlap: true,
		weekNumbers: true,
		forceEventDuration: true,
		eventOverlap: true,
		lazyFetching: true,
		defaultTimedEventDuration: '00:30:00',
		eventRender: function (event, element, view) {
			if (event.description === "yes") {
				element.css('background-color', 'yellow');
			}
			element.attr('href', 'javascript:void(0);');
			element.click(function () {
				$("#startTime").html(moment(event.start).format('MMM Do h:mm A'));
				$("#endTime").html(moment(event.end).format('MMM Do h:mm A'));
				$("#eventLocation").html(event.location);
				$("#eventInfo").html(event.description);
				$("#eventLink").attr('href', event.url);
				$("#eventContent").dialog({
					modal: true, title: event.title, width: 350,
					appendTo: "#calendarModal",
					buttons: [
						{
							text: "Close",
							click: function () {
								$(this).dialog("close");
							}
						}
					],
					closeOnEscape: false,
					open: function (event, ui) {
						$(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
					}
				});
			});
		},
		eventAfterRender: function (event, element) {
			if (event.LeadId !== null && isOverlapping(event) && overLapWarning) {
				alert("Find an overlapped event on " + moment(event.start).format("DD/MM/YYYY"));
				overLapWarning = false;
			}
		},
		viewRender: function (view, element) {
			if ($('#existingjob-switch').prop('checked')) {
				$('#calendar').fullCalendar('removeEventSources');
				$('#calendar').fullCalendar('addEventSource', currentEventsSource);
				window.setTimeout(loadLocalDataSource, 200);
			}
			else
				removeDataSource();
		},
		dayClick: function (date, allDay, jsEvent, view) {
		},
		eventClick: function (calEvent, jsEvent, view) {
		}
	});

	//Render events on calendar
	$('#calendarbtn').click(function () {
		overLapWarning = true;
		currentEventsSource = getCurrentEvents();
		$('#calendar').fullCalendar('removeEventSources');
		if (currentEventsSource.length !== 0) {
			$('#calendar').fullCalendar('addEventSource', currentEventsSource);
			window.setTimeout(clickToday, 200);
			if ($('#existingjob-switch').prop('checked')) {
				if (existingEventsSource === null)
					window.setTimeout(getJsonDataSource, 300);
				else
					loadLocalDataSource();
			}
		}
		else {
			alert("No event be generated! please change end date!");
			return false;
		}

		if ($('#recurring-switch').prop('checked')) {
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
		}
	});

	$('#existingjob-switch').change(function () {
		if ($('#existingjob-switch').prop('checked')) {
			$('#calendar').fullCalendar('removeEventSources');
			$('#calendar').fullCalendar('addEventSource', currentEventsSource);
			loadLocalDataSource();
		}
		else
			removeDataSource();
	});

	function getJsonDataSource() {
		jsonDataSource = getExitingSchedules().OneYearDuration;
		$('#calendar').fullCalendar('addEventSource', jsonDataSource);
		window.setTimeout(setLocalDataSource, 500);
	}

	function loadLocalDataSource() {
		$('#calendar').fullCalendar('addEventSource', existingEventsSource);
	}

	function removeDataSource() {
		var eventSources = $('#calendar').fullCalendar('getEventSources');
		var eventSource = $('#calendar').fullCalendar('getEventSourceById', existingScheduleID);
		if (eventSource !== null)
			$('#calendar').fullCalendar('removeEventSource', jsonDataSource);
		$('#calendar').fullCalendar('removeEventSource', existingEventsSource);
	}

	function setLocalDataSource() {
		var stored = localStorage.getItem('ExitingSchedules');
		if (stored !== null) {
			var result = JSON.parse(stored);
			var events = [];
			for (var i in result)
				events.push(result[i]);
			existingEventsSource = events;
		}
		else {
			alert("Network buys, please try again! ");
		}
	}

	window.getExitingSchedules = function () {
		overLapWarning = true;
		var jobID = $('#ID').val() !== null ? $('#ID').val() : 0;
		var exitingSchedules = {
			OneYearDuration: {
				url: $('#calendar-url').val(),
				cache: true,
				type: 'GET',
				id: existingScheduleID,
				title: "exsiting event",
				dataType: 'json',
				data: { jobID: jobID },
				success: function (result) {
					var events = [];
					$.each(result, function (i, item) {
						var event = {
							id: result[i].ID.toString(),
							title: result[i].Title,
							start: moment(result[i].StartTime, "YYYY-MM-DD HH:mm").format(),
							end: moment(result[i].EndTime, "YYYY-MM-DD HH:mm").format(),
							allDay: false,
							editable: false,
							//	overlap: true,
							className: 'requests',
							color: result[i].Color
						};

						events.push(event);
					});
					localStorage.setItem('ExitingSchedules', JSON.stringify(events));
					return events;
				},
				error: function () {
					alert('There was an error while fetching events!');
				}
			}
		};
		return exitingSchedules;
	};

	function isOverlapping(event) {
		var array = $('#calendar').fullCalendar('clientEvents');
		for (i in array) {
			if (array[i].id !== event.id) {
				if (!(array[i].start >= event.end || array[i].end <= event.start)) {
					return true;
				}
			}
		}
		return false;
	}

	window.getCurrentEvents = function () {
		var events = [];
		var title = $('#job-title').val() === "" ? "Ability First Event" : $('#job-title').val();
		var location = $("#full-address").val() === "" ? "location" : $("#full-address").val();
		if ($('#recurring-switch').prop('checked'))
			events = loadRecurringEvents(events, title, location);
		else
			events = loadNoRecurringEvent(events, title, location);
		return events;
	}

	function loadNoRecurringEvent(events, title, location) {
		//no recurring
		var uid = generateGuid();
		var startDateTime, endDateTime = null;
		var startDate, endDate, startTime, endTime = "";

		startDateTime = $("#datepicker-jobdate").data("DateTimePicker").date();
		endDateTime = startDateTime;
		if (startDateTime !== null)
			startDate = startDateTime.format('YYYY-MM-DD');
		endDate = startDate;
		startTime = $("#timepicker-jobstart").data("DateTimePicker").date();
		endTime = $("#timepicker-jobend").data("DateTimePicker").date();

		if (startTime !== null) {
			var dateString = startDate + " " + startTime.format('HH:mm A');
			startDateTime = moment(dateString, "YYYY-MM-DD HH:mm").format();
		}

		if (endTime !== null) {
			var dateStr = endDate + " " + endTime.format('HH:mm A');
			endDateTime = moment(dateStr, "YYYY-MM-DD HH:mm").format();
		}

		var event = {
			id: uid,
			title: title,
			start: startDateTime,
			end: endDateTime,
			location: location,
			allDay: false
		};
		events.push(event);
		//get first date
		eventsdatearray = [];
		eventsdatearray.push(moment(startDate).format("DD/MM/YYYY"));
		$('#EventsDate').val(eventsdatearray);

		return events;
	}

	function loadRecurringEvents(events, title, location) {
		//recurrence
		var index, day = 0;
		var recurrence, allDates;
		var lastday = false;
		var weekdayend = false;
		var startDateTime, endDateTime = null;
		var startDate, endDate, startTime, endTime = "";
		var frequency = $('#frequency-selecting').val();

		startDateTime = $("#datepicker-jobstart-recurring").data("DateTimePicker").date();
		if (startDateTime !== null)
			startDate = startDateTime.format('YYYY-MM-DD');

		var sequenceweek = $("#sequence-weekly").val();
		var sequencemonth = $("#sequence-monthly").val();
		var occurrence = $('#occurrence-recurring').val();

		if ($('#datepicker-jobend-recurring').is(":visible") && $("#enddate-by").is(":checked"))
			endDateTime = $("#datepicker-jobend-recurring").data("DateTimePicker").date();
		else if ($("#enddate-none").is(":checked")) {
			var duration = moment.duration({ 'year': 3 });
			endDateTime = moment(startDateTime).add(duration);
		}
		else if ($("#enddate-after").is(":checked")) {
			if (frequency === 'Weekly')
				endDateTime = getOccurrenceWeeklyEndDateTime(startDateTime, sequenceweek, occurrence);
			else {
				// frequence and occurence   <<<  have error when select date < startdate
				var monthnumber = parseInt(occurrence);
				if (parseInt(sequencemonth) > 1)
					monthnumber = monthnumber * parseInt(sequencemonth) - 1;
				var month = moment.duration({ 'month': monthnumber });
				endDateTime = moment(startDateTime).add(month);
				endDateTime = moment(endDateTime).endOf('month');
			}
		}

		recurrence = moment().recur(startDateTime.format('YYYY-MM-DD'), endDateTime.format('YYYY-MM-DD'));

		if (endDateTime !== null)
			endDate = endDateTime.format('YYYY-MM-DD');

		startTime = $("#timepicker-jobstart-recurring").data("DateTimePicker").date().format('HH:mm');
		endTime = $("#timepicker-jobend-recurring").data("DateTimePicker").date().format('HH:mm');

		if (frequency === 'Weekly') {
			//weekly recurring - Every [sequenceweek] weeks on [selecteddayofweek]
			var selecteddayofweek = [];
			$('#dayofweek-container input[type=checkbox]').each(function () {
				if (this.checked) {
					var id = this.id.substr('dayofweek'.length);
					selecteddayofweek.push(id);
				}
			});
			recurrence.every(selecteddayofweek).daysOfWeek();
			if (parseInt(sequenceweek) > 1) {
				var weeks = getWeeksOfYear(startDateTime, endDateTime, sequenceweek);
				recurrence.every(weeks).weeksOfYear();
			}
			allDates = recurrence.all("L");
			addEventsOnCalendar(allDates);
		}
		else {
			//monthly recurring - Every [sequencemonth] Months on [frequencymonth] [dayofmonthly/dayofweek]
			var frequencymonth = $("#frequency-monthly").children("option:selected").text();
			var frequencyvalue = $("#frequency-monthly").children("option:selected").val();
			var dayofmonth = "";
			var dayofweek = $("#dayofweek-monthly").children("option:selected").text();
			switch (frequencymonth) {
				case "Day":
					dayofmonth = $("#day-monthly").val();
					allDates = getDatesByDayRecurringMonthly(dayofmonth);
					addEventsOnCalendar(allDates);
					break;
				default:  //first to last   
					if (dayofweek === 'Weekday' || dayofweek === 'Weekend Day') {
						allDates = getDatesByDayOfWeeekMonthlyEvents(frequencymonth, dayofweek, parseInt(frequencyvalue) - 1, parseInt(occurrence));
						addEventsOnCalendar(allDates);
					}
					else if (dayofweek === "Day") {
						if (parseInt(frequencyvalue) === 5)
							dayofmonth = '31';
						else
							dayofmonth = frequencyvalue;
						allDates = getDatesByDayRecurringMonthly(dayofmonth);
						addEventsOnCalendar(allDates);
					}
					else {
						// this is for monthly first-fouth Monday-Sunday, etc.
						allDates = getDatesByFirstToLastAndMondayToSundayRecurringMonthly(recurrence);
						addEventsOnCalendar(allDates);
					}
					break;
			}
		}

		function getDatesByDayOfWeeekMonthlyEvents(frequencymonth, dayofweek, number, occurrence) {
			if (frequencymonth === "Last")
				lastday = true;
			switch (dayofweek) {
				case "Day":
					if (lastday)
						recurrence.every(31).daysOfMonth();
					else
						recurrence.every(number + 1).daysOfMonth();
					break;
				case "Weekday":
					recurrence.every([1, 2, 3, 4, 5]).daysOfWeek();   ///<<<<non endday have error
					weekdayend = true;
					break;
				case "Weekend Day":
					recurrence.every([0, 6]).daysOfWeek();           ///<<<<non endday have error
					weekdayend = true;
					break;
				default: //Mon to Sun
					recurrence.every(dayofweek).daysOfWeek().every([number]).weeksOfMonthByDay();
					break;
			}

			if (parseInt(sequencemonth) > 1) {
				var months = getMonthsOfYear(startDateTime, endDateTime, sequencemonth, dayofmonth);
				recurrence.every(months).monthsOfYear();
			}

			//if ($("#enddate-none").is(":checked"))
			//	allDates = recurrence.next(12 * 3, "L");  //no end date, set to 3 years
			//else if ($("#enddate-after").is(":checked"))
			//	allDates = recurrence.next(parseInt(occurrence), "L");
			//else
			allDates = recurrence.all("L");
			if (frequency === 'Monthly' && weekdayend) {
				allDates = eventDatesFilter(allDates, frequencyvalue, dayofweek, occurrence);
			}
			return allDates;
		}

		function getDatesByDayRecurringMonthly(dayofmonth) {
			recurrence.every(dayofmonth).daysOfMonth();
			if (parseInt(sequencemonth) > 1) {
				var months = getMonthsOfYear(startDateTime, endDateTime, sequencemonth, dayofmonth);
				recurrence.every(months).monthsOfYear();
			}
			allDates = recurrence.all("L");
			return allDates;
		}

		function getDatesByFirstToLastAndMondayToSundayRecurringMonthly(recurrence) {
			var allDates;
			var weekNumberInMonth = parseInt(frequencyvalue) - 1;
			if (frequencymonth === "Last") {
				recurrence.every([22, 23, 24, 25, 26, 27, 28, 29, 30, 31]).daysOfMonth();
				recurrence.every(dayofweek).daysOfWeek();
				if (parseInt(sequencemonth) > 1) {
					var months = getMonthsOfYear(startDateTime, endDateTime, sequencemonth, dayofmonth);
					recurrence.every(months).monthsOfYear();
				}
				allDates = recurrence.all("L");
				allDates = getLastEventDatesFilter(allDates, frequencyvalue, dayofweek);
			}
			else {
				var recurrence = moment(startDateTime).recur().every(dayofweek).daysOfWeek().every(weekNumberInMonth).weeksOfMonthByDay();
				var startDateFrom = moment(startDateTime).add(-1, "days").format('YYYY-MM-DD');
				recurrence.fromDate(startDateFrom);
				var months = getMonthsOfYear(startDateTime, endDateTime, sequencemonth, dayofmonth);
				recurrence.every(months).monthsOfYear();
				if ($("#enddate-none").is(":checked"))
					allDates = recurrence.next(12 * 3, "L");
				else if ($("#enddate-after").is(":checked"))
					allDates = recurrence.next(parseInt(occurrence), "L");
				else {
					recurrence.endDate(endDateTime.format('YYYY-MM-DD'));
					allDates = recurrence.all("L");
				}
			}
			return allDates;
		}

		function getLastEventDatesFilter(eventDates, frequencyvalue, dayofweek) {
			var newEventDates = [];
			var yearMonth = "";
			var i = 0;
			while (eventDates[i]) {
				var currentYearMonth = moment(eventDates[i]).format("YYYYMM");
				if (yearMonth !== currentYearMonth && i != 0) {
					newEventDates.push(eventDates[i - 1]);
					if (i === eventDates.length - 1)
						newEventDates.push(eventDates[i]);
				}
				i++;
				yearMonth = currentYearMonth;
			}
			return newEventDates;
		}

		function eventDatesFilter(eventDates, frequencyvalue, dayofweek, occurrence) {
			var newEventDates = [];
			var i = 0;
			var number = 1;
			var month = 0;
			var yearMonth = "";
			var firstWeekNumber = 0;
			var frequencyDayOfMonth = parseInt(frequencyvalue);
			var noDateGot = true;
			var eventCount = eventDates.length;
			while (eventDates[i]) {
				var currentYearMonth = moment(eventDates[i]).format("YYYYMM");
				if (yearMonth !== currentYearMonth) {
					if (lastday && i != 0) {
						newEventDates.push(eventDates[i - 1]);
					}
					yearMonth = currentYearMonth;
					number = 1;
					month++;
				}

				if (!lastday) {
					if (month === 1 && noDateGot) {
						var getDay = nthWeekdayOfMonth(frequencyDayOfMonth, dayofweek, eventDates[i]);
						if (moment(getDay).format("YYYYMMDD") === moment(eventDates[i]).format("YYYYMMDD")) {
							newEventDates.push(eventDates[i]);
							noDateGot = false;
						}
					}
					else if (number < 7 && yearMonth === currentYearMonth && number === frequencyDayOfMonth) {
						var getDay = nthWeekdayOfMonth(frequencyDayOfMonth, dayofweek, eventDates[i]);
						if (moment(getDay).format("YYYYMMDD") === moment(eventDates[i]).format("YYYYMMDD")) {
							newEventDates.push(eventDates[i]);
						}
					}
				}
				else {
					if (i === eventCount - 1)
						newEventDates.push(eventDates[i]);
				}
				if ($("#enddate-after").is(":checked") && (newEventDates.length >= occurrence))
					break;
				number++;
				i++;
			}
			return newEventDates;
		}

		function nthWeekdayOfMonth(n, weekday, date) {
			var count = 0;
			var idate = moment(date).startOf('month').format("YYYY-MM-DD");
			var y = moment(idate).year();
			var m = moment(idate).months();
			var weekdayNumber = 1;
			var weekenddayNumber = 1;
			while (true) {
				var currentDayOfWeek = moment(idate).days();
				if (weekday === "Weekday") {
					if (currentDayOfWeek >= 1 && currentDayOfWeek <= 5) {
						if (weekdayNumber === n)
							break;
						else {
							if (weekdayNumber > 5)
								weekdayNumber = 1;
							else
								weekdayNumber++;
						}
					}
				}
				else {
					if (currentDayOfWeek === 0 || currentDayOfWeek === 6) {
						if (weekenddayNumber === n)
							break;
						weekenddayNumber++;
					}
				}
				var d = moment(idate).date() + 1;
				idate = moment({ year: y, months: m, days: d });
			}
			return idate;
		}

		function addEventsOnCalendar(allDatesArray) {
			var index = 0;
			var uid = generateGuid();
			var count = getNumberOfFrequency(frequency);
			var j = 0;
			var eventDate;
			eventsdatearray = [];
			while (allDatesArray[day]) {
				if (frequency === 'Monthly' && day >= parseInt(occurrence) && $("#enddate-after").is(":checked"))
					break;
				eventDate = moment(allDatesArray[day]).format("YYYY-MM-DD");
				var eventStartDateTime = moment(eventDate + " " + startTime, "YYYY-MM-DD HH:mm").format();
				var eventEndDateTime = moment(eventDate + " " + endTime, "YYYY-MM-DD HH:mm").format();
				var event = {
					id: index,
					title: title,
					start: eventStartDateTime,
					end: eventEndDateTime,
					location: location,
					allDay: false
				};
				events.push(event);
				//get first frequency date list
				if (j < count)
					eventsdatearray.push(moment(eventDate).format("DD/MM/YYYY"));
				day++;
				index++;
				j++;
			}
			eventsdatearray.push(moment(eventDate).format("DD/MM/YYYY"));
			$('#EventsDate').val(eventsdatearray);
			return events;
		}

		function getOccurrenceWeeklyEndDateTime(startDateTime, sequenceweek, occurrence) {
			var weeknumber = parseInt(occurrence);
			weeknumber = weeknumber * parseInt(sequenceweek);
			var saturday = moment(startDateTime).day("Saturday");
			var duration = moment.duration({ 'week': parseInt(weeknumber) });
			var endDateTime = moment(saturday).add(duration);
			duration = moment.duration({ 'days': moment(startDateTime).day() - 7 });
			endDateTime = moment(endDateTime).add(duration);
			return endDateTime;
		}

		function getWeeksOfYear(startDateTime, endDateTime, sequenceweek) {
			var weeknumber = moment(startDateTime).week();
			var lasteventweeknumber = moment(endDateTime).week();
			if ($('#enddate-none').is(":visible"))
				lasteventweeknumber = weeknumber - 1;
			var weeks = [];
			weeks.push(weeknumber);
			var i = weeknumber;
			while (i <= lasteventweeknumber) {
				i = i + parseInt(sequenceweek);
				if (i <= lasteventweeknumber)
					weeks.push(i);
			}
			if (weeknumber > lasteventweeknumber) {
				i = weeknumber;
				while (i <= 52) {
					i = i + parseInt(sequenceweek);
					if (i <= 52)
						weeks.push(i);
				}
				i = 0;
				while (i <= lasteventweeknumber) {
					i = i + parseInt(sequenceweek);
					if (i <= lasteventweeknumber)
						weeks.push(i);
				}
			}
			return weeks;
		}

		function getMonthsOfYear(startDateTime, endDateTime, sequencemonth, dayofmonth) {
			var monthnumber = moment(startDateTime).month();
			var startdate = moment(startDateTime).date();
			if (parseInt(dayofmonth) < parseInt(startdate) && dayofmonth != "")
				monthnumber = monthnumber + 1;
			var lasteventmonthnumber = moment(endDateTime).month();
			if ($('#enddate-none').is(":visible"))
				lasteventmonthnumber = monthnumber - 1;
			var months = [];
			months.push(monthnumber);
			var i = monthnumber;
			while (i <= lasteventmonthnumber) {
				i = i + parseInt(sequencemonth);
				if (i <= lasteventmonthnumber)
					months.push(i);
			}
			if (monthnumber > lasteventmonthnumber) {
				i = monthnumber;
				while (i < 12) {
					i = i + parseInt(sequencemonth);
					if (i < 12)
						months.push(i);
				}
				i = lasteventmonthnumber + 1;
				while (i >= 0) {
					i = i - parseInt(sequencemonth);
					if (i >= 0)
						months.push(i);
				}
			}
			return months;
		}

		function getNumberOfFrequency(frequency) {
			var number = 0;
			if (frequency === 'Weekly') {
				$('#dayofweek-container input[type=checkbox]').each(function () {
					if (this.checked) {
						number++;
					}
				});
			}
			else {
				number = 1;
			}
			return number;
		}

		return events;
	}

	function clickToday() {
		$('.fc-today-button').click();
	}

	function generateGuid() {
		var guid = Math.random().toString(36).substring(2, 15) +
		Math.random().toString(36).substring(2, 15);
		return guid.toUpperCase();
	}

});