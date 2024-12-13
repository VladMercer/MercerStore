import { useDispatch } from 'react-redux';
import { useEffect, useRef } from 'react';
import { fetchProducts, setPageNumber, setIsPageReset } from '../redux/categorySlice';
import { useCategoryProducts } from './useCategoryProducts';
import { useCategoryPriceRange } from './useCategoryPriceRange';

const useFetchCategoryProducts = () => {
    const dispatch = useDispatch();
    const { selectedMinPrice, selectedMaxPrice, isPriceRangeLoaded } = useCategoryPriceRange();
    const { categoryId, pageNumber, pageSize, sortOrder, isLoaded, isPageReset } = useCategoryProducts();

    const prevSortOrder = useRef(sortOrder);
    const prevMinPrice = useRef(selectedMinPrice);
    const prevMaxPrice = useRef(selectedMaxPrice);

    const dispatchFecthProducts = () => {
        dispatch(fetchProducts({
            categoryId,
            pageNumber,
            pageSize,
            sortOrder,
            minPrice: selectedMinPrice,
            maxPrice: selectedMaxPrice,
        }));
    }

    useEffect(() => {
        if (categoryId && isPriceRangeLoaded) {

            if (!isLoaded) {
                dispatchFecthProducts();
            }

            else if (pageNumber > 1 && (sortOrder !== prevSortOrder.current ||
                selectedMinPrice !== prevMinPrice.current ||
                selectedMaxPrice !== prevMaxPrice.current) && !isPageReset) {
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
            prevMinPrice.current = selectedMinPrice;
            prevMaxPrice.current = selectedMaxPrice;
        }
    }, [isPriceRangeLoaded, pageNumber, pageSize, sortOrder, selectedMinPrice, selectedMaxPrice]);
};

export default useFetchCategoryProducts;