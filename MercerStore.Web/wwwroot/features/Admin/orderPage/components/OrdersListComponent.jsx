import React from 'react';
import { useOrders } from '../hooks/useOrders';
import useFetchOrders from '../hooks/useFetchOrders';

const OrdersListComponent = () => {
    useFetchOrders();
    const { orders } = useOrders();

    return (
        <table className="table table-striped table-hover table-responsive-md table-sm">
            <thead className="thead-dark">
                <tr>
                    <th>ID</th>
                    <th>Контакты</th>
                    <th className="text-center">Адрес</th>
                    <th className="text-center">Даты</th>
                    <th className="text-center">Цена</th>
                    <th className="text-center">Статус</th>
                </tr>
            </thead>
            <tbody>
                {orders && orders.length > 0 ? (
                    orders.map((order) => (
                        <tr
                            key={order.id}
                            onClick={() => (window.location.href = `/admin/order/update/${order.id}`)}
                            style={{ cursor: 'pointer', verticalAlign: 'middle' }}
                        >
                            <td>{order.id}</td>

                            <td>
                                <div>
                                    <strong>Телефон:</strong> {order.phoneNumber}
                                </div>
                                {order.email && (
                                    <div>
                                        <strong>Email:</strong> {order.email}
                                    </div>
                                )}
                            </td>

                            <td className="text-truncate text-wrap" style={{ maxWidth: '150px' }}>
                                {order.address}
                            </td>

                            <td className="text-center">
                                <div>
                                    <strong>Создан:</strong> {new Date(order.createDate).toLocaleDateString()}
                                </div>
                                {order.completedDate && (
                                    <div>
                                        <strong>Завершен:</strong> {new Date(order.completedDate).toLocaleDateString()}
                                    </div>
                                )}
                            </td>
                          
                            <td className="text-center">{order.totalOrderPrice} ₽</td>

                            <td className="text-center">{order.status}</td>
                        </tr>
                    ))
                ) : (
                    <tr>
                        <td colSpan="6" className="text-center">
                            Нет данных
                        </td>
                    </tr>
                )}
            </tbody>
        </table>
    );
};

export default OrdersListComponent;