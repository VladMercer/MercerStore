import React, { useEffect } from 'react';
import { useCart } from '../hooks/useCart';


const CartPageComponent = () => {
    const { items: cartItems, totalPrice, removeFromCart} = useCart();
    return (
        <div className="container-fluid">
            <div className="row">
                <div className="col-12">
                    <nav className="breadcrumbs">
                        <ul>
                            <li><a href="/">Главная</a></li>
                            <li><span>Корзина</span></li>
                        </ul>
                    </nav>
                </div>
            </div>

            <div className="row">
                <div className="col-lg-8 mb-3">
                    <div className="cart-content p-3 h-100 bg-white">
                        <div className="table-responsive">
                            <table className="table align-middle table-hover">
                                <thead className="table-grey">
                                    <tr>
                                        <th>Картинка</th>
                                        <th>Продукт</th>
                                        <th>Стоимость</th>
                                        <th>Кол-во</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {cartItems.length > 0 ? (
                                        cartItems.map(item => (
                                            <tr key={item.productId} className="cart-tr-middle-text">
                                                <td className="product-img-td">
                                                    <a href={`/Product/Details/${item.productId}`} className="cart-a-hover">
                                                        <img src={item.imageUrl} alt={item.name} />
                                                    </a>
                                                </td>
                                                <td>
                                                    <a href={`/Product/Details/${item.productId}`} className="cart-content-title">{item.name}</a>
                                                </td>
                                                <td>{item.price} ₽</td>
                                                <td>{item.quantity}</td>
                                                <td>
                                                    <button
                                                        className="remove-from-cart-button"
                                                        onClick={() => removeFromCart(item.productId)}>
                                                        <i className="fa-solid fa-trash-can"></i>
                                                    </button>
                                                </td>
                                            </tr>
                                        ))
                                    ) : (
                                        <tr>
                                            <td colSpan="5" className="text-center">Корзина пуста</td>
                                        </tr>
                                    )}
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <td colSpan="5" className="text-end">
                                            <a href="/" className="btn btn-outline-warning">Продолжить покупки</a>
                                        </td>
                                    </tr>
                                </tfoot>
                            </table>
                        </div>
                    </div>
                </div>

                <div className="col-lg-4 mb-3">
                    <div className="cart-summary p-3 sidebar">
                        <h5><span>Корзина</span></h5>

                        <div className="d-flex justify-content-between my-3">
                            <h6>Общая стоимость</h6>
                            <h6>{totalPrice} ₽</h6>
                        </div>

                        <div className="d-flex justify-content-between my-3 border-bottom">
                            <h6>Доставка</h6>
                           
                        </div>

                        <button className="btn btn-link px-0 btn-coupon" data-bs-toggle="collapse" data-bs-target="#collapseCoupon">
                            Есть купон?
                        </button>

                        <div className="input-group collapse" id="collapseCoupon">
                            <input type="text" className="form-control" placeholder="Coupon Code" />
                            <button className="btn btn-outline-warning">
                                <i className="fa-regular fa-circle-check"></i>
                            </button>
                        </div>

                        <div className="d-flex justify-content-between my-3">
                            <h3>Итого</h3>
                            <h3>{totalPrice} ₽</h3>
                        </div>

                        <div className="d-grid">
                            <a href="/Order" className="btn btn-outline-warning">Оформить заказ</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default CartPageComponent;