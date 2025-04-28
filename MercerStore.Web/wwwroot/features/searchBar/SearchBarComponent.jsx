import React, {useEffect, useRef} from 'react';

const SearchBarComponent = ({query, setQuery, results, isDropdownVisible, setDropdownVisible, handleSubmit}) => {
    const dropdownRef = useRef(null);

    useEffect(() => {
        const handleClickOutside = (event) => {
            if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
                setDropdownVisible(false);
            }
        };

        document.addEventListener('mousedown', handleClickOutside);
        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
        };
    }, [setDropdownVisible]);

    return (
        <div className="position-relative" ref={dropdownRef}>
            <form onSubmit={handleSubmit} className="d-flex input-group">
                <input
                    type="text"
                    className="form-control"
                    value={query}
                    onChange={(e) => setQuery(e.target.value)}
                    placeholder="Поиск"
                    aria-label="Search"
                    aria-describedby="button-search"
                    onFocus={() => setDropdownVisible(true)}
                />
                <button className="btn btn-outline-warning" type="submit" id="button-search">
                    <i className="fa-solid fa-magnifying-glass"></i>
                </button>
            </form>
            {isDropdownVisible && results.length > 0 && (
                <div className="search-dropdown position-absolute bg-white border rounded shadow-sm w-100">
                    {results.map((product) => (
                        <div
                            key={product.id}
                            className="search-result-item d-flex align-items-center p-2"
                            onClick={() => (window.location.href = `/product/details/${product.id}`)}
                            style={{cursor: 'pointer'}}
                        >
                            <img src={product.mainImageUrl} alt={product.name} className="me-2"
                                 style={{width: 50, height: 50, objectFit: 'cover'}}/>
                            <div>
                                <h5 dangerouslySetInnerHTML={{__html: product.name}}></h5>
                                <p>{product.description}</p>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default SearchBarComponent;