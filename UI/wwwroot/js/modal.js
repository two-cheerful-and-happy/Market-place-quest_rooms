const overlay = document.querySelector("#overlay");
const modalBtn = document.querySelector("#show-modal");
const closeBtn = document.querySelector(".close-popup");
const modal = document.querySelector(".popup");
const modalEmail = document.querySelector(".popup-email");

function closeModal() {
    overlay.style.display = "none";
    modal.classList.remove("active");
    modalEmail.classList.remove("active");
    modalEmail.style.zIndex = -10;
    modalEmail.innerHTML = "";
}



function openModalForm(parameters) {
    const id = parameters.data;
    const url = parameters.url;

    if (id === undefined || url === undefined) {
        alert('Error')
        return;
    }

    axios.get(url + '/' + id)
        .then(function (response) {
            const modalBody = modal.querySelector(".modal-body");
            modalBody.innerHTML = response.data;
            overlay.style.display = "block";
            modal.classList.add("active");
        })
        .catch(function (error) {
            console.log(error);
        });
};


function openModalChangePassword(parameters) {
    const id = parameters.data;
    const url = parameters.url;

    if (id === undefined || url === undefined) {
        alert('Error')
        return;
    }

    axios.get(url + '?code=' + id)
        .then(function (response) {
            const modalBody = modal.querySelector(".modal-body");
            modalBody.innerHTML = response.data;
            overlay.style.display = "block";
            modal.classList.add("active");
        })
        .catch(function (error) {
            console.log(error);
        });
};


function updateDataOfTable(parameters) {

    axios.get(parameters.url)
        .then(function (response) {
        })
        .catch(function (error) {
            console.log(error);
        });
};

function openRequestToCahngeModal(parameter) {
    const url = parameter.url;

    if (url === undefined) {
        alert('Error')
        return;
    }

    axios.get(url)
        .then(function (response) {
            const modalBody = modal.querySelector(".modal-body");
            modalBody.innerHTML = response.data;
            overlay.style.display = "block";
            modal.classList.add("active");
        })
        .catch(function (error) {
            console.log(error);
        });
};

function openRequestToCahngeEmail(parameter) {
    const url = parameter.url;
    if (url === undefined) {
        alert('Error')
        return;
    }

    axios.get(url)
        .then(function (response) {
            modalEmail.style.zIndex = 999;
            modalEmail.innerHTML = response.data;
            overlay.style.display = "block";
            modalEmail.classList.add("active");
        })
        .catch(function (error) {
            console.log(error);
        });
};
