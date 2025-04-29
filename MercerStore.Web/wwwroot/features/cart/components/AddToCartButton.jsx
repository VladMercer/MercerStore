import React from 'react';

import {useCart} from '../hooks/useCart';


const AddToCartButton = ({productId}) => {
    const {addToCart} = useCart();


    return (
        <button
            className="btn btn-outline-warning"
            onClick={() => addToCart(productId)}>
            <i className="fas fa-shopping-cart"></i> Добавить в корзину
        </button>
    );
};

export default AddToCartButton;