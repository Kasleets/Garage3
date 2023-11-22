// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Note: Successfull messages are displayed using SweetAlert2

function addVehicleSuccess() {
    Swal.fire({
        title: 'Success!',
        text: 'Vehicle added successfully!',
        icon: 'success',
        showConfirmButton: false,
        timer: 1500
    });
}

function editVehicleSuccess() {
    Swal.fire({
        title: 'Update Complete!',
        text: 'Vehicle updated successfully!',
        icon: 'success',
        showConfirmButton: false,
        timer: 1500
    });
}

function deleteVehicleSuccess() {
Swal.fire({
        title: 'Delete Complete!',
        text: 'Vehicle deleted successfully!',
        icon: 'success',
        showConfirmButton: false,
        timer: 1500
    });
}

function parkVehicleSuccess() {
    Swal.fire({
        title: 'Parked!',
        text: 'Vehicle has been successfully parked.',
        icon: 'success',
        showConfirmButton: false,
        timer: 1500
    });
}

function retrieveVehicleSuccess() {
    Swal.fire({
        title: 'Retrieved!',
        text: 'Vehicle has been successfully checked out.',
        icon: 'success',
        showConfirmButton: false,
        timer: 1500
    });
}

// Note: Error messages are displayed using SweetAlert2

function addVehicleError() {
    Swal.fire({
        title: 'Error!',
        text: 'Vehicle could not be added.',
        icon: 'error',
        confirmButtonText: 'OK'
    });
}

function editVehicleError() {
    Swal.fire({
        title: 'Error!',
        text: 'Vehicle could not be updated.',
        icon: 'error',
        confirmButtonText: 'OK'
    });
}

function deleteVehicleError() {
    Swal.fire({
        title: 'Error!',
        text: 'Vehicle could not be deleted.',
        icon: 'error',
        confirmButtonText: 'OK'
    });
}

function parkVehicleError() {
    Swal.fire({
        title: 'Error!',
        text: 'Vehicle could not be parked.',
        icon: 'error',
        confirmButtonText: 'OK'
    });
}

function retrieveVehicleError() {
    Swal.fire({
        title: 'Error!',
        text: 'Vehicle could not be retrieved.',
        icon: 'error',
        confirmButtonText: 'OK'
    });
}

function vehicleNotFound() {
    Swal.fire({
        title: 'Error!',
        text: 'Vehicle not found.',
        icon: 'error',
        confirmButtonText: 'OK'
    });
}

function vehicleAlreadyParked() {
    Swal.fire({
        title: 'Error!',
        text: 'Vehicle is already parked.',
        icon: 'error',
        confirmButtonText: 'OK'
    });
}

function vehicleAlreadyRetrieved() {
    Swal.fire({
        title: 'Error!',
        text: 'Vehicle is already retrieved.',
        icon: 'error',
        confirmButtonText: 'OK'
    });
}

function vehicleAlreadyExists() {
    Swal.fire({
        title: 'Error!',
        text: 'Vehicle already exists.',
        icon: 'error',
        confirmButtonText: 'OK'
    });
}

function vehicleNotFound() {
    Swal.fire({
        title: 'Error!',
        text: 'Vehicle not found.',
        icon: 'error',
        confirmButtonText: 'OK'
    });
}

// Note: Login form https://sweetalert2.github.io/recipe-gallery/login-form.html
// Todo: bind to relevant inputs from Views

type LoginFormResult = {
    username: string
    password: string
}

let usernameInput: HTMLInputElement
let passwordInput: HTMLInputElement

Swal.fire < LoginFormResult > ({
    title: 'Login Form',
    html: `
    <input type="text" id="username" class="swal2-input" placeholder="Username">
    <input type="password" id="password" class="swal2-input" placeholder="Password">
  `,
    confirmButtonText: 'Sign in',
    focusConfirm: false,
    didOpen: () => {
        const popup = Swal.getPopup()!
        usernameInput = popup.querySelector('#username') as HTMLInputElement
        passwordInput = popup.querySelector('#password') as HTMLInputElement
        usernameInput.onkeyup = (event) => event.key === 'Enter' && Swal.clickConfirm()
        passwordInput.onkeyup = (event) => event.key === 'Enter' && Swal.clickConfirm()
    },
    preConfirm: () => {
        const username = usernameInput.value
        const password = passwordInput.value
        if (!username || !password) {
            Swal.showValidationMessage(`Please enter username and password`)
        }
        return { username, password }
    },
})
