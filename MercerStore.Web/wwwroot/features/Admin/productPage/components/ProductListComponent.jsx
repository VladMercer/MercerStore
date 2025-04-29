import React from 'react';
import {useProducts} from '../hooks/useProducts';
import useFetchProducts from '../hooks/useFetchProducts';

const ProductListComponent = () => {
    useFetchProducts();
    const {products} = useProducts();

    return (
        <table className="table table-striped table-hover table-responsive-md table-sm">
            <thead className="thead-dark">
            <tr>
                <th className="text-center">Картинка</th>
                <th>Название</th>
                <th className="text-center">Цена / Скидка</th>
                <th className="text-center">Статус и Наличие</th>
            </tr>
            </thead>
            <tbody>
            {products && products.length > 0 ? (
                products.map((product) => (
                    <tr
                        key={product.id}
                        onClick={() => (window.location.href = `/admin/product/update/${product.id}`)}
                        style={{cursor: 'pointer', verticalAlign: 'middle'}}
                    >

                        <td className="text-center">
                            <img
                                src={product.mainImageUrl}
                                alt={product.name}
                                className="img-fluid"
                                style={{maxWidth: '50px', height: 'auto'}}
                                loading="lazy"
                            />
                        </td>

                        <td className="text-truncate text-wrap" style={{maxWidth: '200px'}}>
                            <p
                                className="m-0"
                                dangerouslySetInnerHTML={{
                                    __html: product.name,
                                }}
                            ></p>
                        </td>


                        <td className="text-center">
                            <div className="d-flex flex-column">
                                <span>{product.price}₽</span>
                                {product.discountedPrice && (
                                    <small className="text-muted">
                                        {product.discountedPrice}₽
                                    </small>
                                )}
                            </div>
                        </td>


                        <td className="text-center">
                            <div className="d-flex flex-column align-items-center">
                                <span>{product.status}</span>
                                <span className="mt-2">
                                        {product.inStock > 0
                                            ? `${product.inStock} шт.`
                                            : 'Нет в наличии'}
                                    </span>
                            </div>
                        </td>
                    </tr>
                ))
            ) : (
                <tr>
                    <td colSpan="4" className="text-center">
                        Нет данных
                    </td>
                </tr>
            )}
            </tbody>
        </table>
    );
};

export default ProductListComponent;