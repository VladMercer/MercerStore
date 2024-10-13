import React, { useContext } from 'react';
import { toast, Slide } from 'react-toastify';
import { CartContext } from './CartContext';

const AddToCartButton = ({ productId }) => {
    const { addToCart } = useContext(CartContext);

    const handleAddToCart = async () => {
        try {
            await addToCart(productId);
            toast.success('Товар успешно добавлен в корзину!', {
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
            console.error('Ошибка при добавлении товара:', error);
        }
    };

    return (
        <button
            className={`btn btn-outline-warning`}
            onClick={handleAddToCart}
        >
            <i className="fas fa-shopping-cart"></i> Добавить в корзину
        </button>
    );
};

export default AddToCartButton;