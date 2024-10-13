import React from 'react';
import ReactDOM from 'react-dom';
import { SearchProvider } from './logic/searchPage/SearchContext';
import { CartProvider } from './cart/CartContext';
import { ProductProvider } from './logic/categoryPage/ProductContext';
import SearchAppWithPortals from './logic/searchPage';
import CartAppWithPortals from './cart';
import CategoryAppWithPortals from './logic/categoryPage';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';


const MainApp = () => (
   
    <CartProvider>
        <ProductProvider>
            <SearchProvider>
                <CartAppWithPortals />
                <CategoryAppWithPortals />
                <SearchAppWithPortals />
                <ToastContainer />
            </SearchProvider>
        </ProductProvider>
    </CartProvider>
);

ReactDOM.render(<MainApp />, document.getElementById('root'));