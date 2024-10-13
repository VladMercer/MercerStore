import React, { useContext } from 'react';
import { CartContext } from './CartContext';
import CartPageComponent from './CartPageComponent';

const CartPageApp = () => {
    const { cartItems, totalPrice, removeFromCart } = useContext(CartContext);

    return (
        <CartPageComponent
            cartItems={cartItems}
            totalPrice={totalPrice}
            onRemoveFromCart={removeFromCart}
        />
    );
};

export default CartPageApp;