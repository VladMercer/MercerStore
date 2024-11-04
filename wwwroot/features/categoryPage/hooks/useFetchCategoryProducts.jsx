import { useSelector, useDispatch } from 'react-redux';
import { useEffect } from 'react';
import { fetchProducts } from '../redux/categorySlice';

export const useFetchCategoryProducts = () => {
    const dispatch = useDispatch();
    const products = useSelector((state) => state.category.products);
    const isLoaded = useSelector((state) => state.category.isLoaded);
    const pageNumber = useSelector((state) => state.category.pageNumber);
    const pageSize = useSelector((state) => state.category.pageSize);
    const sortOrder = useSelector((state) => state.category.sortOrder);

    useEffect(() => {
        if (!isLoaded) {
            dispatch(fetchProducts({ pageNumber, pageSize, sortOrder }));
        }
    }, [isLoaded, dispatch, pageNumber, pageSize, sortOrder]);

    return products;
};