function addToCart(variantId, quantity) {
    var token = $('input[name="__RequestVerificationToken"]').val();

    return $.ajax({
        url: '/Cart/AddToCart', 
        type: 'POST',
        data: {
            variantId: variantId,
            quantity: quantity,
            __RequestVerificationToken: token
        },
        success: function (response) {
            if (response.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Done!',
                    text: response.message,
                    timer: 2000
                });
                $("#cart-badge-count").text(response.cartCount);
            } else {
                Swal.fire('Error', response.message, 'error');
            }
        },
        error: function (xhr) {
            if (xhr.status === 401) {
                Swal.fire('Login Required', 'Please login first', 'info').then(() => {
                    window.location.href = "/Auth/Account/Login";
                });
            } else {
                Swal.fire('Error', 'Something went wrong', 'error');
            }
        }
    });
}