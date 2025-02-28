import React from 'react';
import AddToCartButton from '../../cart/components/AddToCartButton';
import { useCategoryProducts } from '../../categoryPage/hooks/useCategoryProducts';
import useFetchCategoryProducts from '../hooks/useFetchCategoryProducts';
const ProductListComponent = () => {
    useFetchCategoryProducts();
    const { products } = useCategoryProducts();

    return (
        <div className="row">
            {products && products.length > 0 ? (
                products.map(product => (
                    <div className="col-lg-4 col-sm-6 mb-3" key={product.id}>
                        <div className="product-card">
                            <div className="product-thumb">
                                <a href={`/product/details/${product.id}`}>
                                    <img src={product.mainImageUrl} alt="фотокарточка" />
                                </a>
                            </div>
                            <div className="product-details">
                                <h4>
                                    <a
                                        href={`/product/details/${product.id}`}
                                        dangerouslySetInnerHTML={{ __html: product.name }}
                                    ></a>
                                </h4>
                                <p className="product-excerpt">{product.description}</p>
                                <div className="product-bottom-details d-flex justify-content-between align-items-center">
                                    {product.discountedPrice ? (
                                        <div className="price-container">
                                            <small className="old-price-card">
                                                {product.price.toLocaleString()}₽
                                            </small>
                                            <div className="product-price">
                                                {product.discountedPrice.toLocaleString()}₽
                                            </div>
                                        </div>
                                    ) : (
                                        <div className="product-price">
                                            {product.price.toLocaleString()}₽
                                        </div>
                                    )}
                                    <AddToCartButton productId={product.id} />
                                </div>
                            </div>
                        </div>
                    </div>
                ))
            ) : (
                <p>Нет продуктов</p>
            )}
        </div>
    );
};

export default ProductListComponent;