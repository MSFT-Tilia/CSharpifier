
int main(int argc, char** argv)
{

	if (!rootFrame->Navigate(MainPage::typeid, argument))
	{}

	int c = co_await check();
	int k = c+1;
	return k;
}