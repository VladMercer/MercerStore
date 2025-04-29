import React, {useContext} from 'react';
import {CartContext} from './CartContext';
import CartHeaderComponent from './CartHeaderComponent';

const CartHeaderApp = () => {
    const {totalPrice, itemCount} = useContext(CartContext);

    return (
        <CartHeaderComponent totalPrice={totalPrice} itemCount={itemCount}/>
    );
};

export default CartHeaderApp;