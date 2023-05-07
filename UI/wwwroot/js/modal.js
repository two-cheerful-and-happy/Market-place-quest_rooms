const overlay = document.querySelector("#overlay");
const modalBtn = document.querySelector("#show-modal");
const closeBtn = document.querySelector(".close-popup");
const modal = document.querySelector(".popup");

modalBtn.addEventListener("click", function() {
    overlay.style.display = "block";
    modal.classList.add("active");
});

closeBtn.addEventListener("click", function() {
    overlay.style.display = "none";
    modal.classList.remove("active");
});

overlay.addEventListener("click", function() {
    overlay.style.display = "none";
    modal.classList.remove("active");
});



function openModalForm(parameters) {
    const id = parameters.data;
    const url = parameters.url;
    const overlay = document.querySelector("#overlay");
    const modal = document.querySelector(".popup");
    console.log(modal);

    if (id === undefined || url === undefined) {
        alert('Error')
        return;
    }

    axios.get(url + '/' + id)
        .then(function (response) {
            const modalBody = modal.querySelector(".modal-body");
            modalBody.innerHTML = response.data;
            overlay.style.display = "block";
            /*modal.modal('show').css('z-index', 9999);*/
            modal.classList.add("active");
        })
        .catch(function (error) {
            console.log(error);
        });
};

function openRequestToCahngeModal(parameter) {

    const modal = $('#modal');

    if (parameter === undefined) {
        alert('Error')
        return;
    }

    axios.get(parameter.url)
        .then(function (response) {
            modal.find(".modal-body").html(response.data);
            modal.modal('show').css('z-index', 9999);
        })
        .catch(function (error) {
            console.log(error);
        });
};

function openModalSearch(parameter) {

    const modal = $('#modal');
    modal

    if (parameter === undefined) {
        alert('Error')
        return;
    }

    axios.get(parameter.url)
        .then(function (response) {
            modal.find(".modal-body").html(response.data);
            modal.modal('show').css('z-index', 9999);
        })
        .catch(function (error) {
            console.log(error);
        });
};