import React, { useContext, useEffect } from 'react';
import { ProductContext } from './ProductContext';
import { CartContext } from '../../cart/CartContext';
import ProductListComponent from '../../UI/ProductListComponent';

const ProductListApp = () => {
    const { products, fetchProducts } = useContext(ProductContext);
    const { addToCart} = useContext(CartContext)

    useEffect(() => {
        fetchProducts();
    }, []);

    return (
        <ProductListComponent products={products} addToCart={addToCart} />
    )
    
   
};

export default ProductListApp;