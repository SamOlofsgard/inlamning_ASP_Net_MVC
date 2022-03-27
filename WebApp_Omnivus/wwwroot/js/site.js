var forms = document.querySelectorAll('.needs-validation')

function onSubmit(e) {
    e.preventDefault()
    console.log("submitted")
}

function validMinValueTwo(value,) {
    const regEx = /[0-9a-zA-Z]{2,}/
    if (!regEx.test(value))
        return false

    return true
}

function validEmail(value) {
    const regEx = /^(([^<>()[\]\\.,:åäöÅÄÖ\s@\.,;:åäöÅÄÖ\"]+(\.[^<>()[\]\\.,;:åäöÅÄÖ\s@\.,;:åäöÅÄÖ\"]+)*)|(".+"))@(([\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;


    if (!regEx.test(value)) {
        return false
    }



    return true
}

function checkValidForm(elements) {
    let disable = false
    let errors = document.querySelectorAll('.is-invalid')
    let submitButton = document.querySelectorAll('.submit')[0]

    elements.forEach(element => {
        if (element.value === "" || errors.length > 0)
            disable = true
    })

    if (submitButton !== undefined)
        submitButton.disabled = disable
}


function setEventListeners() {
    forms.forEach(element => {

        switch (element.type) {
            case "text":
                element.addEventListener("keyup", function (e) {

                    if (!validMinValueTwo(e.target.value)) {
                        e.target.classList.add("is-invalid");

                        checkValidForm(forms)
                    }
                    else {
                        e.target.classList.remove("is-invalid");

                        checkValidForm(forms)

                    }


                })
                break;

            case "textarea":
                element.addEventListener("keyup", function (e) {

                    if (!validMinValueTwo(e.target.value)) {
                        e.target.classList.add("is-invalid");

                        checkValidForm(forms)
                    }
                    else {
                        e.target.classList.remove("is-invalid");

                        checkValidForm(forms)

                    }


                })
                break;

            case "email":
                element.addEventListener("keyup", function (e) {
                    if (!validEmail(e.target.value)) {
                        e.target.classList.add("is-invalid");
                        checkValidForm(forms)
                    }
                    else {
                        e.target.classList.remove("is-invalid");
                        checkValidForm(forms)
                    }
                })
                break;

        }

    })
}

setEventListeners()
checkValidForm(forms)
