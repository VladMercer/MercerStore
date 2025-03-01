import { useDispatch } from 'react-redux';
import { useEffect, useRef } from 'react';
import { fetchProducts, setPageNumber, setIsPageReset, fetchCategories } from '../redux/adminProductPageSlice';
import { useProducts } from './useProducts';


const useFetchProducts = () => {
    const dispatch = useDispatch();
    const { categoryId, pageNumber, pageSize, sortOrder, filter, isLoaded, isPageReset } = useProducts();

    const prevSortOrder = useRef(sortOrder);

    const dispatchFecthProducts = () => {
        dispatch(fetchProducts({
            categoryId,
            pageNumber,
            pageSize,
            sortOrder,
            filter
        }));
    }

    useEffect(() => {
        

            if (!isLoaded) {
                dispatchFecthProducts();
                dispatch(fetchCategories());
            }

            else if (pageNumber > 1 && (sortOrder !== prevSortOrder.current) && !isPageReset) {
                dispatch(setPageNumber(1));
                dispatch(setIsPageReset(true));
            }

            else if (!isPageReset || (isPageReset && pageNumber === 1)) {

                dispatchFecthProducts();

                if (isPageReset && pageNumber === 1) {
                    dispatch(setIsPageReset(false));
                }
            }

            prevSortOrder.current = sortOrder;
    }, [categoryId, pageNumber, pageSize, sortOrder, filter]);
};

export default useFetchProducts;