import React from 'react';
import { createPortal } from 'react-dom';
import CartPageApp from './CartPageApp';
import CartHeaderApp from './CartHeaderApp';
import CartOffcanvasApp from './CartOffcanvasApp';
import AddToCartButton from './AddToCartButton';

const cartHeaderRoot = document.getElementById('cart-header-root');
const cartPageRoot = document.getElementById('cart-page-root');
const cartOffcanvasRoot = document.getElementById('cart-offcanvas-root');
const cartAddToCartButtonRoots = document.querySelectorAll('[id^="add-to-cart-root-"]');

const CartAppWithPortals = () => {
    return (
        <>
          
            {cartAddToCartButtonRoots.length > 0 &&
                Array.from(cartAddToCartButtonRoots).map((root) => {
                    const productId = root.getAttribute('data-product-id');
                    return createPortal(
                        <AddToCartButton productId={productId} />,
                        root
                    );
                })
            }

            {cartOffcanvasRoot && createPortal(<CartOffcanvasApp />, cartOffcanvasRoot)}
            {cartHeaderRoot && createPortal(<CartHeaderApp />, cartHeaderRoot)}
            {cartPageRoot && createPortal(<CartPageApp />, cartPageRoot)}
        </>
    );
};

export default CartAppWithPortals;