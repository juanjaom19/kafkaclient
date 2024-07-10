document.addEventListener('DOMContentLoaded', function() {
    document.getElementById('addButton').addEventListener('click', function() {
        // alert('Agregar botón clickeado');
        console.log('Agregar botón clickeado');
    });

    document.getElementById('deleteButton').addEventListener('click', function() {
        // alert('Eliminar botón clickeado');
        console.log('Eliminar botón clickeado');
    });

    var toggler = document.getElementsByClassName('caret');
    for (var i = 0; i < toggler.length; i++) {
        toggler[i].addEventListener('click', function() {
            this.parentElement.querySelector('.nested').classList.toggle('active');
            this.classList.toggle('caret-down');
            
            var icon = this.querySelector('.icon i');
            if (this.parentElement.querySelector('.nested').classList.contains('active')) {
                icon.classList.remove('fa-plus-square');
                icon.classList.add('fa-minus-square');
            } else {
                icon.classList.remove('fa-minus-square');
                icon.classList.add('fa-plus-square');
            }

            // var icon = this.querySelector('i');
            // if (this.parentElement.querySelector('.nested').classList.contains('active')) {
            //     if (icon.classList.contains('fa-folder')) {
            //         icon.classList.remove('fa-folder');
            //         icon.classList.add('fa-folder-open');
            //     }
            // } else {
            //     if (icon.classList.contains('fa-folder-open')) {
            //         icon.classList.remove('fa-folder-open');
            //         icon.classList.add('fa-folder');
            //     }
            // }
        });
    }
});