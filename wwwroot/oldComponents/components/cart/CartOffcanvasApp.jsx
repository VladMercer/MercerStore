import React, { useContext } from 'react';
import { CartContext } from './CartContext';
import CartOffcanvasComponent from './CartOffcanvasComponent';

const CartOffcanvasApp = () => {
    const { cartItems, removeFromCart, totalPrice } = useContext(CartContext);

    return (
        <CartOffcanvasComponent
            cartItems={cartItems}
            removeFromCart={removeFromCart}
            totalPrice={totalPrice}
        />
    );
};

export default CartOffcanvasApp;