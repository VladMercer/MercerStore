import { useSelector, useDispatch } from 'react-redux';
import { addToCart, removeFromCart } from '../redux/cartSlice';

export const useCart = () => {
    const dispatch = useDispatch();
    const { items, totalPrice, itemCount } = useSelector((state) => state.cart);

    return {
        items,
        totalPrice,
        itemCount,
        addToCart: (productId) => dispatch(addToCart(productId)),
        removeFromCart: (productId) => dispatch(removeFromCart(productId)),
    };
};