import React, {useContext} from 'react';
import {SearchContext} from './SearchContext';
import {CartContext} from '../../cart/CartContext';
import ProductListComponent from '../../UI/ProductListComponent';

const ProductListApp = () => {
    const {results} = useContext(SearchContext);
    const {addToCart} = useContext(CartContext)

    return (
        <ProductListComponent products={results} addToCart={addToCart}/>
    );
};

export default ProductListApp;