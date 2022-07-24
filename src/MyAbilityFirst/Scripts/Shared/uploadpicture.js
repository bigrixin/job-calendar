jQuery.noConflict()(function ($) {
	$(document).ready(function () {
		$('#uploadPicture').fileupload({
			dataType: 'json',
			url: $('#uploadURL').prop('value'),
			autoUpload: true,

			add: function (e, data) {
				var uploadErrors = [];
				var fileType = data.files[0].name.split('.').pop(), allowdtypes = 'jpeg,jpg,png,gif,JPEG,JPG,PNG,GIF,MP4,mp4';
				if (allowdtypes.indexOf(fileType) < 0) {
					uploadErrors.push('Invalid file type, aborted');
				}
				//5000000 --> 5M
				if (data.originalFiles[0]['size'] > 5000000) {
					uploadErrors.push('File size is too big');
				}
				if (uploadErrors.length > 0) {
					alert(uploadErrors.join('\n'));
				} else {
					data.submit();
				}
			},
			start: function (e, data) {
				$('#uploadPictureBar').css('width','5%');
				$('#uploadPictureBar').text('5% complete');
			},
			progress: function (e, data) {
				var progress = parseInt(data.loaded / data.total * 100, 10);

				if (progress < 80) {
					$('#uploadPictureBar').css('width', progress + '%');
					$('#uploadPictureBar').text(progress + '% complete');
				}
			},
			fail: function (event, data) {
				if (data.files[0].error) {
					var thisId = $(this).attr('id');
					var resultId = 'result' + thisId[thisId.length - 1];
					$('#' + resultId).html(data.files[0].error);
				}
			},
			done: function (e, data) {
				var thisId = $(this).attr('id');
				var resultId = 'result' + thisId[thisId.length - 1];
				var cancelId = 'cancelUpload' + thisId[thisId.length - 1];
				var barId = 'bar' + thisId[thisId.length - 1];
				if (data.result.isUploaded) {

					$('#uploadPictureBar').css('width', '100%');
					$('#uploadPictureBar').text('100% complete');

					$('#cancelUploadPicture').prop('disabled', false);

					//write in EditorFor
					var fileRecordId = 'fileRecord' + thisId[thisId.length - 1];

					$('#pictureRecord').attr('value', data.result.file);

					$('#' + thisId).prop('disabled', true);
					//show picture
					//readURL(this);
					$('#pictureFileId').attr('src', data.result.file);
				}
				else {
					$('#uploadPictureBar').css('width', '0%');
					$('#uploadPictureBar').text('0% complete');
				}

			}
		});

		//check upload file status

		var fileName = $('#pictureRecord').val();

		if (fileName === null || fileName === '') {
			$('#cancelUploadPicture').prop('disabled', true);

			$('#uploadPicture').prop('disabled', false);
			$('#uploadPictureBar').css('width', '0%');
			$('#uploadPictureBar').text('0% complete');
		}
		else {
			$('#cancelUploadPicture').prop('disabled', false);
			$('#uploadPicture').prop('disabled', true);
			$('#uploadPictureBar').css('width', '100%');
			$('#uploadPictureBar').text('100% complete');
		}
	});

	//preview picture
	function readURL(input) {
		if (input.files && input.files[0]) {
			var reader = new FileReader();

			reader.onload = function (e) {
				$('#pictureFileId').attr('src', e.target.result);
			};

			reader.readAsDataURL(input.files[0]);
		}
	}

	//clean file name
	$('#cancelUploadPicture').click(function () {
		var fileName = $('#pictureRecord').val();
		// delete file from server
		$.ajax({
			dataType: 'json',
			url: $('#deleteURL').prop('value'),
			type: 'DELETE',
			data: { 'fileName': fileName },
			success: function (result) {
				///	alert(result.message);
				$('#uploadPictureBar').css('width', '0%');
				$('#uploadPictureBar').text('');
				//clear file name
				$('#pictureRecord').attr('value', '');
				$('#uploadPicture').prop('disabled', false);
				//disable cancel button
				$('#cancelUploadPicture').prop('disabled', true);
				$('#pictureFileId').attr('src', '');
			},
			error: function () {
				alert('error');
			}
		});

	});

});

//file type
function isImage(filename) {
	var ext = getExtension(filename);
	switch (ext.toLowerCase()) {
		case 'jpg':
		case 'gif':
		case 'bmp':
		case 'png':
			//etc
			return true;
	}
	return false;
}

function getExtension(filename) {
	var parts = filename.split('.');
	return parts[parts.length - 1];
}