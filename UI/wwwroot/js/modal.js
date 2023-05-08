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


function closeModal() {
    overlay.style.display = "none";
    modal.classList.remove("active");
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

function updateDataOfTable(parameters) {

    axios.get(parameters.url)
        .then(function (response) {
        })
        .catch(function (error) {
            console.log(error);
        });
};


function openRequestToCahngeModal(parameter) {
    const url = parameters.url;

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
