import React from 'react';
import AddToCartButton from '../cart/AddToCartButton';


const ProductListComponent = ({products}) => {
    return (
        <div className="row">
            {products.length > 0 ? (
                products.map(product => (
                    <div className="col-lg-4 col-sm-6 mb-3" key={product.id}>
                        <div className="product-card">
                            <div className="product-thumb">
                                <a href={`/product/details/${product.id}`}>
                                    <img src={product.mainImageUrl} alt="фотокарточка"/>
                                </a>
                            </div>
                            <div className="product-details">
                                <h4>
                                    <a href={`/product/details/${product.id}`}
                                       dangerouslySetInnerHTML={{__html: product.name}}></a>
                                </h4>
                                <p className="product-excerpt">{product.description}</p>
                                <div className="product-bottom-details d-flex justify-content-between">
                                    <div className="product-price">{product.price}₽</div>
                                    <AddToCartButton productId={product.id}/>
                                </div>
                            </div>
                        </div>
                    </div>
                ))
            ) : (
                <p>No products found</p>
            )}
        </div>
    );
};

export default ProductListComponent;