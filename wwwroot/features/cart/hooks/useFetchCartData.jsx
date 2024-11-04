import { useSelector, useDispatch } from 'react-redux';
import { useEffect } from 'react';
import { fetchCartData } from '../redux/cartSlice';

const useFetchCartData = () => {
    const dispatch = useDispatch();
    const { items, isLoaded } = useSelector((state) => state.cart);

    useEffect(() => {
        if (!isLoaded) {
            dispatch(fetchCartData());
        }
    }, [isLoaded, dispatch]);

    return items;
};
export default useFetchCartData;