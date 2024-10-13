import React, { createContext, useState, useEffect } from 'react';
import axios from 'axios';
import { toast, Slide } from 'react-toastify';
export const CartContext = createContext();

export const CartProvider = ({ children }) => {
    const [cartItems, setCartItems] = useState([]);
    const [totalPrice, setTotalPrice] = useState(0);
    const [itemCount, setItemCount] = useState(0);

    const fetchCartData = async () => {
        try {
            const response = await axios.get(`/CartApi/GetProducts`);
            const newCartItems = response.data.cartItems.map(item => ({ ...item }));
            const newTotalPrice = response.data.cartTotalPrice;
            const newItemCount = response.data.cartItemCount;

            setCartItems([...newCartItems]);  
            setTotalPrice(newTotalPrice);     
            setItemCount(newItemCount);       

        } catch (error) {
            console.error('Ошибка при получении данных корзины:', error);
        }
    };

    const addToCart = async (productId) => {
        try {
            await axios.post(`/CartApi/AddToCart?quantity=1&productId=${productId}`);
            await fetchCartData();

        } catch (error) {
            console.error('Ошибка при добавлении товара в корзину в addToCart:', error);
        }
    };

    const removeFromCart = async (productId) => {
        try {

            await axios.post(`/CartApi/RemoveFromCart?productId=${productId}`);

            await fetchCartData();
             toast.success('Товар успешно удален из корзины!', {
                position: "bottom-right",
                autoClose: 3000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: false,
                draggable: true,
                progress: undefined,
                theme: "colored",
                transition: Slide,
                className: 'my-custom-toast',  
                bodyClassName: 'my-custom-body', 
                progressClassName: 'my-custom-progress-bar' 
             });

        } catch (error) {
            console.error('Ошибка при удалении товара из корзины в removeFromCart:', error);
        }
    };

    useEffect(() => {
        fetchCartData();
    }, []);

    return (
        <CartContext.Provider value={{
            itemCount,
            totalPrice,
            cartItems, 
            addToCart,
            removeFromCart,
            fetchCartData
        }}>
            {children}
        </CartContext.Provider>
    );
};