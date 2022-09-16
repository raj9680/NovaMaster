// prevent enter 
$(document).ready(function () {
    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });
// validate only numbers
$('.numbersonly').keypress(function (e) {
    var a = [];
    var k = e.which;

    for (i = 48; i < 58; i++)
        a.push(i);

    if (!(a.indexOf(k) >= 0))
        e.preventDefault();
});
// validate only letters
$('.textonly').keydown(function (e) {
    if (e.shiftKey || e.ctrlKey || e.altKey) {
        e.preventDefault();
    } else {
        var key = e.keyCode;
        if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90))) {
            e.preventDefault();
        }
    }
});
});


// validate email
function validateEmail($email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    return emailReg.test($email);
}

// validate passport
$('#ClientDocs_Passport').bind('change', function () {
    // extension
    var passportType = ['png','jpg','pdf','jpeg'];
    if ($.inArray($(this).val().split('.').pop().toLowerCase(), passportType) == -1) {
        alert("Only 'png/jpg/pdf' format is allowed.");
        this.value = ''; // Clean field
        return false;
    }

    // fileSize
    var passportSize = this.files[0].size;
    if (passportSize > 3145728) {
        $('#ClientDocs_Passport').val(null);
        alert("Passport file size should not exceed 3MB");
        return false;
    }
});

// validate Exam(IELTS, TOFEL etc.)
$('#ClientDocs_EnglishExam').bind('change', function () {
    // extension
    var englishExam = ['png', 'jpg', 'pdf', 'jpeg'];
    if ($.inArray($(this).val().split('.').pop().toLowerCase(), englishExam) == -1) {
        alert("Only 'png/jpg/pdf' format is allowed.");
        this.value = ''; // Clean field
        return false;
    }

    // fileSize
    var englishExamSize = this.files[0].size;
    if (englishExamSize > 3145728) {
        $('#ClientDocs_EnglishExam').val(null);
        alert("Exam(IELTS, TOFEL etc.) file size should not exceed 3MB");
        return false;
    }
});

// validate Matriculation
$('#ClientDocs_Matriculation').bind('change', function () {
    // extension
    var matriculationExam = ['png', 'jpg', 'pdf', 'jpeg'];
    if ($.inArray($(this).val().split('.').pop().toLowerCase(), matriculationExam) == -1) {
        alert("Only 'png/jpg/pdf' format is allowed.");
        this.value = ''; // Clean field
        return false;
    }

    // fileSize
    var matriculationSize = this.files[0].size;
    if (matriculationSize > 3145728) {
        $('#ClientDocs_Matriculation').val(null);
        alert("Matriculation file size should not exceed 3MB");
        return false;
    }
});

// validate Secondary
$('#ClientDocs_SeniorSecondary').bind('change', function () {
    // extension
    var secondaryExam = ['png', 'jpg', 'pdf', 'jpeg'];
    if ($.inArray($(this).val().split('.').pop().toLowerCase(), secondaryExam) == -1) {
        alert("Only 'png/jpg/pdf' format is allowed.");
        this.value = ''; // Clean field
        return false;
    }

    // fileSize
    var secondarySize = this.files[0].size;
    if (secondarySize > 3145728) {
        $('#ClientDocs_SeniorSecondary').val(null);
        alert("Secondary file size should not exceed 3MB");
        return false;
    }
});

// validate Bachelors
$('#ClientDocs_BachelorsDegree').bind('change', function () {
    // extension
    var bachelorsExam = ['png', 'jpg', 'pdf', 'jpeg'];
    if ($.inArray($(this).val().split('.').pop().toLowerCase(), bachelorsExam) == -1) {
        alert("Only 'png/jpg/pdf' format is allowed.");
        this.value = ''; // Clean field
        return false;
    }

    // fileSize
    var bachelorsSize = this.files[0].size;
    if (bachelorsSize > 3145728) {
        $('#ClientDocs_BachelorsDegree').val(null);
        alert("Bachelors file size should not exceed 3MB");
        return false;
    }
});

// validate Bachelors
$('#ClientDocs_WorkExperience').bind('change', function () {
    // extension
    var workExperience = ['png', 'jpg', 'pdf', 'jpeg'];
    if ($.inArray($(this).val().split('.').pop().toLowerCase(), workExperience) == -1) {
        alert("Only 'png/jpg/pdf' format is allowed.");
        this.value = ''; // Clean field
        return false;
    }

    // fileSize
    var workExperienceSize = this.files[0].size;
    if (workExperienceSize > 3145728) {
        $('#ClientDocs_WorkExperience').val(null);
        alert("Bachelors file size should not exceed 3MB");
        return false;
    }
});

function first() {
    var fullName = $("#Users_FullName").val();
    var email = $("#Users_Email").val();
    var password = $("#Users_Password").val();
    var cnfPassword = $("#Users_CnfPassword").val();
    if (fullName == "") {
        alert("Full name required");
        return false;
    }
    if (fullName.length < 3) {
        alert("Full name length should not be less than 3")
        return false;
    }
    if (email == "") {
        alert("Email required");
        return false;
    }
    if (email.length <= 3) {
        alert("Invalid email");
        return false;
    }
    if (email.length > 3) {
        if (!validateEmail(email)) {
            alert("Invalid email address");
            return false;
        }
    }
    
    if (password == "") {
        alert("Password required");
        return false;
    }
    if (cnfPassword == "") {
        alert("Confirm Password required");
        return false;
    }
    if (password.length < 6) {
        alert("Password length should be 6 atleast");
        return false;
    }
    if (password != cnfPassword) {
        alert("Password and confirm password does not match");
        return false;
    }

    var res = IsAlreadyExist(email);
    if (res) {
        return false;
    }
    else {
        var active = $('.wizard .nav-tabs li.active');
        active.next().removeClass('disabled');
        nextTab(active);
        return true;
    }
}

function second() {
    var contactNumber = $("#StudentsInfo_ContactNumber").val();
    var dob = $("#StudentsInfo_DOB").val();
    var address1 = $("#StudentsInfo_AddressLine1").val();
    var address2 = $("#StudentsInfo_AddressLine2").val();
    var city = $("#city").val();
    var zip = $("#StudentsInfo_Zip").val();
    if (contactNumber == "") {
        alert("Contact number required");
        return false;
    }
    if (contactNumber.length < 10 || contactNumber.length > 15) {
        alert("Contact number length should not be less than or greater than 15");
        return false;
    }
    if (dob == "") {
        alert("DOB required");
        return false;
    }
    if (address1 == "") {
        alert("Address 1 required");
        return false;
    }
    if (address1.length > 60) {
        alert("Address 1 length should not exceed 60 characters");
        return false;
    }
    if (address2.length > 60) {
        alert("Address 2 length should not exceed 60 characters");
        return false;
    }
    if (city == -1 || city == "") {
        alert("Please select city");
        return false;
    }
    if (zip == "") {
        alert("Zip code required");
        return false;
    }
    if (zip.length != 6) {
        alert("Invalid zip code");
        return false;
    }
    var active = $('.wizard .nav-tabs li.active');
    active.next().removeClass('disabled');
    nextTab(active);
    return true
}

function third() {
    var primaryLanguage = $("#StudentsInfo_PrimaryLanguage").val();
    var englishExamType = $("#StudentsInfo_EnglishExamType").val();
    var intake = $("#StudentsInfo_Intake").val();
    var program = $("#StudentsInfo_Program").val();
    var programPreference = $("#StudentsInfo_ProgramCollegePreference").val();
    if (primaryLanguage == -1 || primaryLanguage == "") {
        alert("Please choose primary language");
        return false;
    }
    if (englishExamType == -1 || englishExamType == "") {
        alert("Please choose english exam type");
        return false;
    }
    if (intake == -1 || intake == "") {
        alert("Please choose intake to apply");
        return false;
    }
    if (program == -1 || program == "") {
        alert("Please choose program to apply");
        return false;
    }
    if (programPreference.length > 60) {
        alert("College preference field required");
        return false;
    }
    var active = $('.wizard .nav-tabs li.active');
    active.next().removeClass('disabled');
    nextTab(active);
    return true;
}

function fourth() {
    var higherEducation = $('#StudentsInfo_HighestEducation').val();
    if (higherEducation == "" || higherEducation == "-1") {
        alert("Please select your highest level of education");
        return false;
    }
    var val = higherEducation;
    // Validate Matriculation
    if (val == "Matriculation") {
        var IsMa = matriculation_required();
        if (IsMa) {
            var active = $('.wizard .nav-tabs li.active');
            active.next().removeClass('disabled');
            nextTab(active);
            return true;
        }
        return false;
    }

    // Validate Secondary
    if (val == "Senior Secondary") {
        var isMat = matriculation_required();
        if (isMat) {
            var IsSec = secondary_required();
            if (IsSec) {
                var active = $('.wizard .nav-tabs li.active');
                active.next().removeClass('disabled');
                nextTab(active);
                return true;
            }
        }
        return false;
    }

    // Validate Bachelors
    if (val == "Bachelors Degree") {
        var isMat2 = matriculation_required();
        if (isMat2) {
            var iss = secondary_required();
            if (iss) {
                var issBa = bachelors_required();
                if (issBa) {
                    var active = $('.wizard .nav-tabs li.active');
                    active.next().removeClass('disabled');
                    nextTab(active);
                    return true;
                }
                return false;
            }
        }
        return false;
    }

    // Validate Masters
    if (val == "Masters Degree") {
        var isMat3 = matriculation_required();
        if (isMat3) {
            var isSec2 = secondary_required();
            if (isSec2) {
                var isBat = bachelors_required();
                if (isBat) {
                    var mass = masters_required();
                    if (mass) {
                        var active = $('.wizard .nav-tabs li.active');
                        active.next().removeClass('disabled');
                        nextTab(active);
                        return true;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }
        return false;
    }
}

function fifth() {
        if (first()) {
            if (second()) {
                if (third()) {
                    if (fourth()) {
                        var companyName = $('input[name="StudentsInfo.CompanyName"]').val();
                        var jobTitle = $('input[name="StudentsInfo.JobTitle"]').val();
                        var jobStartDate = $('input[name="StudentsInfo.JobStartDate"]').val();
                        var jobEndDate = $('input[name="StudentsInfo.JobEndDate"]').val();
                        var isRefusedVisa = $("#StudentsInfo_IsRefusedVisa").val();
                        var explainIfRefused = $('input[name="StudentsInfo.ExplainIfRefused"]').val();
                        var haveStudyPermit = $('#StudentsInfo_HaveStudyPermitVisa').val();
                        if (companyName != "" || companyName.length != 0) {
                            if (jobTitle == "") {
                                return alert("Job title required");
                            }
                            if (jobStartDate == "") {
                                return alert("Job start date required");
                            }
                            if (jobEndDate == "") {
                                return alert("Job end date required");
                            }
                        }
                        if (isRefusedVisa == "" || isRefusedVisa == "-1") {
                            return alert("Please select any field (Have you refused any visa?)");
                        }
                        if (isRefusedVisa != "" && isRefusedVisa != "-1" && isRefusedVisa == "true" && explainIfRefused == "" && explainIfRefused.length < 1) {
                            return alert("Please provide details (Why you've refused visa?)");
                        }
                        if (haveStudyPermit == "" || haveStudyPermit == "-1") {
                            return alert("Please select (Do you've valid study permit/visa?)");
                        }
                        localStorage.removeItem("selectedEdu");
                        localStorage.setItem("selectedEdu", $("#StudentsInfo_HighestEducation option:selected").text())
                        $('#submit').trigger('click');
                        return true;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }
    return false;
}

$(document).ready(function () {
    $('.nav-tabs > li a[title]').tooltip();

    //Wizard
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var target = $(e.target);
        if (target.parent().hasClass('disabled')) {
            return false;
        }
    });

    /*
    $(".next-step").click(function (e) {
    var active = $('.wizard .nav-tabs li.active');
    active.next().removeClass('disabled');
    nextTab(active);
    });
    */
    $(".prev-step").click(function (e) {
    var active = $('.wizard .nav-tabs li.active');
    prevTab(active);
    });
    
});

function nextTab(elem) {
    $(elem).next().find('a[data-toggle="tab"]').click();
}

function prevTab(elem) {
    $(elem).prev().find('a[data-toggle="tab"]').click();
}

$('.nav-tabs').on('click', 'li', function () {
    $('.nav-tabs li.active').removeClass('active');
    $(this).addClass('active');
});

// Validations
$(document).ready(function () {
    if (localStorage.getItem('selectedEdu') == 'Matriculation') {
        $('#StudentsInfo_HighestEducation option[value="Matriculation"]').attr("selected", "selected");
        $("[id='matriculation']").show();
        $("[id='secondary']").hide();
        $("[id='bachelors']").hide();
        $("[id='masters']").hide();
    }
    else if (localStorage.getItem('selectedEdu') == 'Senior Secondary') {
        $('#StudentsInfo_HighestEducation option[value="Senior Secondary"]').attr("selected", "selected");
        $("[id='matriculation']").show();
        $("[id='secondary']").show();
        $("[id='bachelors']").hide();
        $("[id='masters']").hide();
    }
    else if (localStorage.getItem('selectedEdu') == 'Bachelors Degree') {
        $('#StudentsInfo_HighestEducation option[value="Bachelors Degree"]').attr("selected", "selected");
        $("[id='matriculation']").show();
        $("[id='secondary']").show();
        $("[id='bachelors']").show();
        $("[id='masters']").hide();
    }
    else if (localStorage.getItem('selectedEdu') == 'Masters Degree') {
        $('#StudentsInfo_HighestEducation option[value="Masters Degree"]').attr("selected", "selected");
        $("[id='matriculation']").show();
        $("[id='secondary']").show();
        $("[id='bachelors']").show();
        $("[id='masters']").show();
    }
    else {
        $("[id='matriculation']").hide();
        $("[id='secondary']").hide();
        $("[id='bachelors']").hide();
        $("[id='masters']").hide();
    }

    $('select[name="StudentsInfo.HighestEducation"]').change(function () {
        if ($(this).val() == 'Matriculation') {
            $("[id='matriculation']").show();
            $("[id='secondary']").hide();
            $("[id='bachelors']").hide();
            $("[id='masters']").hide();
            clearSecondary();
            clearBachelors();
            clearMasters();
        } else if ($(this).val() == 'Senior Secondary') {
            $("[id='matriculation']").show();
            $("[id='secondary']").show();
            $("[id='bachelors']").hide();
            $("[id='masters']").hide();
            clearBachelors();
            clearMasters();
        } else if ($(this).val() == 'Bachelors Degree') {
            $("[id='matriculation']").show();
            $("[id='secondary']").show();
            $("[id='bachelors']").show();
            $("[id='masters']").hide();
            clearMasters();
        } else if ($(this).val() == 'Masters Degree') {
            $("[id='matriculation']").show();
            $("[id='secondary']").show();
            $("[id='bachelors']").show();
            $("[id='masters']").show();
        } else {
            $("[id='matriculation']").hide();
            $("[id='secondary']").hide();
            $("[id='bachelors']").hide();
            $("[id='masters']").hide();
        }
    });
});

// Required
function matriculation_required() {
    var matStart = $('input[name="StudentsInfo.MatricEducationStartDate"]').val();
    var matEnd = $('input[name="StudentsInfo.MatricEducationEndDate"]').val();
    var matCompletion = $('input[name="StudentsInfo.MatricEducationCompletionDate"]').val();
    var matInstituteInfo = $('#StudentsInfo_MatricInstituteInfo').val();
    var matPercentage = $('input[name="StudentsInfo.MatricEducationPercentage"]').val();
    if (matStart == "") {
        return alert("Matric start date required");
    }
    if (matEnd == "") {
        return alert("Matric end date required");
    }
    if (matCompletion == "") {
        return alert("Matric completion date required");
    }
    if (matInstituteInfo.length < 1) {
        return alert("Matric institute info. required");
    }
    if (matInstituteInfo.length > 120) {
        return alert("Matric institute info. should not exceed 120 characters");
    }
    if (matPercentage == "") {
        return alert("Matric percentage required");
    }
    return true;
}

function secondary_required() {
    var secStart = $('input[name="StudentsInfo.SecondaryEducationStartDate"]').val();
    var secEnd = $('input[name="StudentsInfo.SecondaryEducationEndDate"]').val();
    var secCompletion = $('input[name="StudentsInfo.SecondaryEducationCompletionDate"]').val();
    var secInstituteInfo = $('#StudentsInfo_SecondaryInstituteInfo').val();
    var secPercentage = $('input[name="StudentsInfo.SecondaryEducationPercentage"]').val();
    if (secStart == "") {
        return alert("Secondary start date required");
    }
    if (secEnd == "") {
        return alert("Secondary end date required");
    }
    if (secCompletion == "") {
        return alert("Secondary completion date required");
    }
    if (secInstituteInfo.length < 1) {
        return alert("Secondary institute info. required");
    }
    if (secInstituteInfo.length > 120) {
        return alert("Secondary institute info. should not exceed 120 characters");
    }
    if (secPercentage == "") {
        return alert("Secondary percentage required");
    }
    return true;
}

function bachelors_required() {
    var batStart = $('input[name="StudentsInfo.BachelorsEducationStartDate"]').val();
    var batEnd = $('input[name="StudentsInfo.BachelorsEducationEndDate"]').val();
    var batCompletion = $('input[name="StudentsInfo.BachelorsEducationCompletionDate"]').val();
    var batInstituteInfo = $('#StudentsInfo_BachelorsInstituteInfo').val();
    var batPercentage = $('input[name="StudentsInfo.BachelorsEducationPercentage"]').val();
    if (batStart == "") {
        alert("Bachelors start date required");
        return false;
    }
    if (batEnd == "") {
        alert("Bachelors end date required");
        return false;
    }
    if (batCompletion == "") {
        alert("Bachelors completion date required");
        return false;
    }
    if (batInstituteInfo.length < 1) {
        alert("Bachelors institute info. required");
        return false;
    }
    if (batInstituteInfo.length > 120) {
        alert("Bachelors institute info. should not exceed 120 characters");
        return false;
    }
    if (batPercentage == "") {
        alert("Bachelors percentage required");
        return false;
    }
    return true;
}

function masters_required() {
    var mastStart = $('input[name="StudentsInfo.MastersEducationStartDate"]').val();
    var mastEnd = $('input[name="StudentsInfo.MastersEducationEndDate"]').val();
    var mastCompletion = $('input[name="StudentsInfo.MastersEducationCompletionDate"]').val();
    var mastInstituteInfo = $('#StudentsInfo_MastersInstituteInfo').val();
    var mastPercentage = $('input[name="StudentsInfo.MastersEducationPercentage"]').val();
    if (mastStart == "") {
        return alert("Masters start date required");
    }
    if (mastEnd == "") {
        alert("Masters end date required");
        return false;
    }
    if (mastCompletion == "") {
        alert("Masters completion date required");
        return false;
    }
    if (mastInstituteInfo.length < 1) {
        alert("Masters institute info. required");
        return false;
    }
    if (mastInstituteInfo.length > 120) {
        alert("Masters institute info. should not exceed 120 characters");
        return false;
    }
    if (mastPercentage == "") {
        alert("Masters percentage required");
        return false;
    }
    return true;
}

function clearSecondary() {
    $('#StudentsInfo_SecondaryEducationStartDate').val('').attr('type', 'text').attr('type', 'date');
    $('#StudentsInfo_SecondaryEducationEndDate').val('').attr('type', 'text').attr('type', 'date');
    $('#StudentsInfo_SecondaryEducationCompletionDate').val('').attr('type', 'text').attr('type', 'date');
    $('#StudentsInfo_SecondaryInstituteInfo').val('').attr('type', 'text');
    $('#StudentsInfo_SecondaryEducationPercentage').val('').attr('type', 'number');
    $('#StudentsInfo_SecondaryEducationMathsmarks').val('').attr('type', 'number');
    $('#StudentsInfo_SecondaryEducationEnglishMarks').val('').attr('type', 'number');
}

function clearBachelors() {
    $('#StudentsInfo_BachelorsEducationStartDate').val('').attr('type', 'text').attr('type', 'date');
    $('#StudentsInfo_BachelorsEducationEndDate').val('').attr('type', 'text').attr('type', 'date');
    $('#StudentsInfo_BachelorsEducationCompletionDate').val('').attr('type', 'text').attr('type', 'date');
    $('#StudentsInfo_BachelorsInstituteInfo').val('').attr('type', 'text');
    $('#StudentsInfo_BachelorsEducationPercentage').val('').attr('type', 'number');
    $('#StudentsInfo_BachelorsEducationMathsmarks').val('').attr('type', 'number');
    $('#StudentsInfo_BachelorsEducationEnglishMarks').val('').attr('type', 'number');
}

function clearMasters() {
    $('#StudentsInfo_MastersEducationStartDate').val('').attr('type', 'text').attr('type', 'date');
    $('#StudentsInfo_MastersEducationEndDate').val('').attr('type', 'text').attr('type', 'date');
    $('#StudentsInfo_MastersEducationCompletionDate').val('').attr('type', 'text').attr('type', 'date');
    $('#StudentsInfo_MastersInstituteInfo').val('').attr('type', 'text');
    $('#StudentsInfo_MastersEducationPercentage').val('').attr('type', 'number');
    $('#StudentsInfo_MastersEducationMathsmarks').val('').attr('type', 'number');
    $('#StudentsInfo_MastersEducationEnglishMarks').val('').attr('type', 'number');
}

// IsExist
$('#Users_Email').on('change', function () {
    $('#state option').remove();
    var CountryId = $('#country option:selected').val();    /* var CountryId = $(this).val(); */
    //alert(CountryId);
    if (CountryId != -1) {
        $.ajax({
            type: 'GET',
            url: '/Common/GetState?CountryId=' + CountryId,
            data: { CountryId: CountryId },
            dataType: 'json',
            success: function (result) {
                var ddlist = $('#state');
                $.each(result, function (val, text) {
                    ddlist.append(
                        $('<option></option>').val(text.value).html(text.text))
                });
            }
        });
    }
});

// If Exist
function IsAlreadyExist(email) {
    var bool = false;
    $.ajax({
        async: true,
        type: 'GET',
        url: '/Client/IsClientExist?ClientEmail=' + email,
        data: { ClientEmail: email }, // dataType: 'json',
        success: function (result) {
            if (result) {
                alert('Email already exist');
                bool = true;
            }
        }
    });
    return bool;
}