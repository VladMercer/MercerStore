import React, {useEffect} from 'react';
import {useCart} from '../hooks/useCart';
import useFetchCartData from '../hooks/useFetchCartData';


const CartOffcanvasComponent = () => {
    const cartItems = useFetchCartData();
    const {totalPrice, removeFromCart} = useCart();
    useEffect(() => {
        const offcanvasCartEl = document.getElementById('offcanvasCart');
        if (offcanvasCartEl) {
            const offcanvasCart = new bootstrap.Offcanvas(offcanvasCartEl);

            document.getElementById('cart-open')?.addEventListener('click', (e) => {
                e.preventDefault();
                offcanvasCart.toggle();
            });
        }
    }, []);
    return (
        <div className="offcanvas offcanvas-end" tabIndex="-1" id="offcanvasCart" aria-labelledby="offcanvasCartLabel">
            <div className="offcanvas-header">
                <h5 className="offcanvas-title" id="offcanvasCartLabel">Корзина</h5>
                <button type="button" className="btn-close" data-bs-dismiss="offcanvas" aria-label="Close"></button>
            </div>
            <div className="offcanvas-body">
                {cartItems.length > 0 ? (
                    <>
                        <div className="table-responsive">
                            <table className="table offcanvasCart-table">
                                <tbody>
                                {cartItems.map((item) => (
                                    <tr key={item.productId}>
                                        <td className="product-img-td">
                                            <a href={`/product/details/${item.productId}`}>
                                                <img src={item.imageUrl} alt={item.name}
                                                     className="img-fluid product-image"/>
                                            </a>
                                        </td>
                                        <td className="product-cart-name-td">
                                            <a href={`/product/details/${item.productId}`}
                                               className="product-name-link">
                                                {item.name}
                                            </a>
                                        </td>

                                        <td className="product-price-td">{item.discountedPrice ? (
                                            <div className="price-container">
                                                <small className="old-price-card">
                                                    {item.price.toLocaleString()}₽
                                                </small>
                                                <div className="">
                                                    {item.discountedPrice.toLocaleString()}₽
                                                </div>
                                            </div>
                                        ) : (
                                            <div className="">
                                                {item.price.toLocaleString()}₽
                                            </div>
                                        )}</td>
                                        <td className="product-quantity-td">&times;{item.quantity}</td>
                                        <td className="remove-btn-td">
                                            <button onClick={() => removeFromCart(item.productId)}
                                                    className="remove-from-cart-button">
                                                <i className="fa-solid fa-trash-can"></i>
                                            </button>
                                        </td>
                                    </tr>
                                ))}
                                </tbody>
                                <tfoot>
                                <tr>
                                    <td colSpan="3" className="text-end-td">Итого:</td>
                                    <td colSpan="2" className="total-price-td">{totalPrice} ₽</td>
                                </tr>
                                </tfoot>
                            </table>
                        </div>
                        <div className="text-end mt-3 d-flex justify-content-between fixed-bottom-buttons">
                            <a href="/cart" className="btn btn-outline-warning" id="button-offcanvas-cart">Перейти в
                                корзину</a>
                            <a href="/order" className="btn btn-outline-warning" id="button-offcanvas-checkout">Оформить
                                заказ</a>
                        </div>
                    </>
                ) : (
                    <p>Ваша корзина пуста</p>
                )}
            </div>
        </div>
    );
};

export default CartOffcanvasComponent;