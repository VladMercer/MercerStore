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
import { ReviewProvider } from './Review/ReviewContext';
import ReviewAppWithPortals from './Review';


const MainApp = () => (
   
    <CartProvider>
        <ProductProvider>
            <SearchProvider>
                <ReviewProvider>
                    <ReviewAppWithPortals />
                    <CartAppWithPortals />
                    <CategoryAppWithPortals />
                    <SearchAppWithPortals />
                    <ToastContainer />
                </ReviewProvider>
            </SearchProvider>
        </ProductProvider>
    </CartProvider>
);

ReactDOM.render(<MainApp />, document.getElementById('root'));