import React from 'react';
const CartHeaderComponent = ({ itemCount, totalPrice }) => {

    return (
        <div>
            <button className="btn p-1" id="cart-open" type="button" data-bs-toggle="offcanvas2"
                data-bs-target="#offcanvasCart" aria-controls="offcanvasCart">
                <i className="fa-solid fa-cart-shopping"></i>
                <span className="badge text-bg-warning cart-badge bg-warning rounded-square">{ itemCount}</span>
                <p className="p-cart-total-price">{totalPrice} ₽</p>
            </button>
        </div>
    );
};

export default CartHeaderComponent;