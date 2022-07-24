jQuery.noConflict()(function ($) {
	$(document).ready(function () {

		//check upload file status
		$('input[type=checkbox]').each(function () {
			if (this.checked) {
				var indexId = $(this).attr('value');
				$('#progress' + indexId).show();
				$('#file' + +indexId).show();
				$('#cancelUpload' + indexId).show();
				$('#preview' + indexId).show();
				$('#preview' + indexId).removeAttr('disabled');
				$('#' + "cancelUpload" + indexId).prop('disabled', false);
				$('#' + "bar" + indexId).css('width', '100%');
				$('#' + "bar" + indexId).text('100% complete');
				$('#' + "file" + indexId).prop('disabled', true);
			}
		});

		$(':checkbox').change(function () {
			var indexId = $(this).attr('value');
			if (this.checked) {
				var labelId = $(this).attr('id');
				var labelValue = $("label[for='" + labelId + "']").text();

				if (labelValue.search("Photo") === 0) {
					$('#file' + indexId).attr('accept', "image/*");
				}

				$('#progress' + indexId).show();
				$('#file' + +indexId).show();
				$('#cancelUpload' + indexId).show();
				$('#preview' + indexId).show();
			}
			else {
				$('#progress' + indexId).hide();
				$('#file' + +indexId).hide();
				$('#cancelUpload' + indexId).hide();
				$('#preview' + indexId).hide();
			}
		});

		$('.fileupload').fileupload({
			dataType: 'json',
			url: $('#uploadURL').prop("value"),
			autoUpload: true,
			add: function (e, data) {
				//data.url =$('#uploadURL').prop("value");
				var uploadErrors = [];
				var fileType = data.files[0].name.split('.').pop(), allowdtypes = 'jpeg,jpg,png,gif,pdf,doc,docx,xls,xlsx';
				if (allowdtypes.indexOf(fileType) < 0) {
					uploadErrors.push('Invalid file type, aborted');
				}
				//5000000 --> 5M
				if (data.originalFiles[0]['size'] > 5000000) {
					uploadErrors.push('File size is too big');
				}
				if (uploadErrors.length > 0) {
					alert(uploadErrors.join("\n"));
				} else {
					var thisId = $(this).attr('id');
					var indexId = parseInt(thisId.substr(4, thisId.length));
					var userId = $('#userId').val();
					data.formData = { "itemId": indexId, "userId": userId };
					data.submit();
				}
			},
			start: function (e, data) {
				var thisId = $(this).attr('id');
				var indexId = thisId.substr(4, thisId.length);
				var barId = "bar" + indexId;
				$('#' + barId).css('width', '5%');
				$('#' + barId).text('5% complete');

			},
			progress: function (e, data) {

				var progress = parseInt(data.loaded / data.total * 100, 10);
				var thisId = $(this).attr('id');
				var indexId = thisId.substr(4, thisId.length);
				// file1 ~ file5, bar1 ~ bar5
				var barId = "bar" + indexId;
				if (progress < 80) {
					$('#' + barId).css('width', progress + '%');
					$('#' + barId).text(progress + '% complete');
				}
			},
			fail: function (event, data) {
				if (data.files[0].error) {
					var thisId = $(this).attr('id');
					var indexId = thisId.substr(4, thisId.length);
					var resultId = "result" + indexId;
					$('#' + resultId).html(data.files[0].error);
					alert("Error:" + data.files[0].error);
				}
			},
			done: function (e, data) {
				var thisId = $(this).attr('id');
				var indexId = thisId.substr(4, thisId.length);
				var resultId = "result" + indexId;
				var cancelId = "cancelUpload" + indexId;
				var barId = "bar" + indexId;
				var previewId = "preview" + indexId;

				if (data.result.statusCode === 200) {
					$('.upload-panel' + indexId).hide();
					$('.preview-panel' + indexId).show();

					$('#' + barId).css('width', '100%');
					$('#' + barId).text('100% complete');

					$('#' + barId).text(data.result.message);
					$('#' + cancelId).prop('disabled', false);

					$('#' + previewId).removeAttr("disabled");
					$('#' + previewId).attr("href", data.result.file);
					$(this).prop('disabled', true);
				}
				else {
					$('#' + barId).css('width', '0%');
					$('#' + barId).text('0 %');
				}
			}
		});

	});

	$("button").click(function () {
		var thisId = $(this).attr('id');
		var indexId = thisId.substr(12, thisId.length);

		//disable cancel button
		$('#' + thisId).prop('disabled', true);
		var barId = "bar" + +indexId;
		$('#' + barId).css('width', '0%');
		$('#' + barId).text('');
		//enable upload file
		var fileId = "file" + indexId;

		var previewId = "preview" + indexId;
		var fileName = $('#' + previewId).attr('href');
		var userId = $('#userId').val();
		// delete file from server
		$.ajax({
			dataType: 'json',
			url: $('#deleteURL').prop("value"),
			type: 'DELETE',
			data: { 'fileName': fileName, 'userId': userId, 'itemId': indexId },
			success: function (result) {
				$('.upload-panel' + indexId).show();
				$('.preview-panel' + indexId).hide();

				$('#' + previewId).attr("href", "#");
				$('#' + previewId).attr("disabled", true);
				$('#' + fileId).removeAttr("disabled"); //prop('disabled', false);
			},
			error: function () {
				alert("error");
			}
		});

	});


});